using System;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using HarmonyLib;
using System.Text;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;


namespace Marathon;

[BepInPlugin("org.ret2plt.Marathon", "Marathon", "1.0.0.0")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    public class SecureAESInstance : IDisposable
    {
        private RijndaelManaged _rijndael;
        private ICryptoTransform _encryptor;
        

        public SecureAESInstance(byte[] key)
        {
            this._rijndael = new RijndaelManaged();
            this._encryptor = this._rijndael.CreateEncryptor(key, this._rijndael.IV);
            key = SymmetricAlgorithm.Create("AES").Key;
            
        }

        public void Dispose()
        {
            _encryptor?.Dispose();
            _rijndael?.Dispose();
        }

        public byte[] Encrypt(byte[] buffer)
        {
            byte[] second = _encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
            return _rijndael.IV.Concat(second).ToArray();
            
        }
        
        
    }

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        
        var harmony = new Harmony("org.ret2plt.Marathon");
        Logger.LogInfo($"Harmony {harmony.Id} is loaded!");

        [HarmonyPatch(typeof(EncryptionHelper.SimpleAes), "Encrypt")]
        [HarmonyPrefix]
        string EncryptPrefix(string input)
        {
            
            SecureAESInstance aes = new SecureAESInstance(SymmetricAlgorithm.Create("AES").Key);
            return Convert.ToBase64String(aes.Encrypt(Encoding.UTF8.GetBytes(input)));
            
        }
        
        //harmony.Patch(
        //    typeof(EncryptionHelper.SimpleAes).GetMethod("Encrypt"),
        //    prefix: new HarmonyMethod(typeof(Plugin).GetMethod("EncryptPrefix"))
        //);
        Logger.LogInfo("Patched vulnerable Encrypt method");
        [HarmonyPatch(typeof(SecurityChecker), "antivirusInstalled")]
        [HarmonyPrefix]
        bool antivirusInstalledPrefix()
        {
            return false;
        }
        //harmony.Patch(
        //    typeof(SecurityChecker).GetMethod("antivirusInstalled"),
        //    prefix: new HarmonyMethod(typeof(Plugin).GetMethod("antivirusInstalledPrefix"))
        //);
        Logger.LogInfo("Patched out antivirusInstalled method");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        
        
    }
}
