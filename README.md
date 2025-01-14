## Marathon

Marathon patches 2/3 of the vulnerabilities described in [this blogpost](https://ret2p.lt/vts-local-vulnerabilities/). It is a BepInEx Mono based patcher built off of branch commit `0c2a3f13c388852177b813c280212d0e23bb5670`.
Marathon is open-source and free-to-use for all, though you can support me via purchasing it [here](https://ko-fi.com/s/dde2e37912). Any support helps, genuinely.

Marathon will be continually updated as I audit the IL codebase (Assembly-CSharp.dll) of VTS. 

### What about model stealing?

Model stealing will come as a SaaS bundle for an "anticheat" for VTS, VBridger, OBS, etc. It'll encompass a WHQL signed driver (first to be signed with an EV certificate). As EV certificates and WHQL certification is expensive, the driver will be sold for the price of $100 USD, per month.
This service will take a while to setup, so be patient. The code will be independently audited both by Microsoft and a third party, with a document published on it. Durandal will have it's own GitHub, containing the FOSS portions of it. The model for Durandal is similar to that of Vanguard,
albeit most of the core logic is in the userland executable (DurandalService) rather than the driver. The driver just ensures the integrity of the executable and also of Marathon (if installed.)
