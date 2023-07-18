##### 2023/07/18 22:26:16 #####

java.lang.ExceptionInInitializerError

--------------------------------

---------- Stack trace ---------

--------------------------------

    com.g.a.a.c.<init>(FreeScrollingTextField.java:128)
    ru.maximoff.apktool.view.Editor.<init>(Editor.java:40)
    ru.maximoff.apktool.fragment.a.b.a(EditorPagerItem.java:491)
    ru.maximoff.apktool.fragment.a.b.<init>(EditorPagerItem.java:91)
    ru.maximoff.apktool.fragment.a.a.a(EditorPagerAdapter.java:122)
    ru.maximoff.apktool.fragment.a.a(EditorFragment.java:298)
    ru.maximoff.apktool.fragment.a.a(EditorFragment.java:290)
    ru.maximoff.apktool.MainActivity$16.run(MainActivity.java:501)
    android.os.Handler.handleCallback(Handler.java:942)
    android.os.Handler.dispatchMessage(Handler.java:99)
    android.os.Looper.loopOnce(Looper.java:211)
    android.os.Looper.loop(Looper.java:300)
    android.app.ActivityThread.main(ActivityThread.java:8152)
    java.lang.reflect.Method.invoke(Native Method)
    com.android.internal.os.RuntimeInit$MethodAndArgsCaller.run(RuntimeInit.java:580)
    com.android.internal.os.ZygoteInit.main(ZygoteInit.java:1028)

--------------------------------

------------- Cause ------------

--------------------------------

java.lang.NullPointerException

    java.io.File.<init>(File.java:283)
    ru.maximoff.apktool.util.j.<init>(EditorTheme.java:17)
    com.g.a.b.c$a.<init>(ColorScheme.java:44)
    com.g.a.b.c$a.<clinit>(Unknown Source:4)
    com.g.a.a.c.<init>(FreeScrollingTextField.java:128)
    ru.maximoff.apktool.view.Editor.<init>(Editor.java:40)
    ru.maximoff.apktool.fragment.a.b.a(EditorPagerItem.java:491)
    ru.maximoff.apktool.fragment.a.b.<init>(EditorPagerItem.java:91)
    ru.maximoff.apktool.fragment.a.a.a(EditorPagerAdapter.java:122)
    ru.maximoff.apktool.fragment.a.a(EditorFragment.java:298)
    ru.maximoff.apktool.fragment.a.a(EditorFragment.java:290)
    ru.maximoff.apktool.MainActivity$16.run(MainActivity.java:501)
    android.os.Handler.handleCallback(Handler.java:942)
    android.os.Handler.dispatchMessage(Handler.java:99)
    android.os.Looper.loopOnce(Looper.java:211)
    android.os.Looper.loop(Looper.java:300)
    android.app.ActivityThread.main(ActivityThread.java:8152)
    java.lang.reflect.Method.invoke(Native Method)
    com.android.internal.os.RuntimeInit$MethodAndArgsCaller.run(RuntimeInit.java:580)
    com.android.internal.os.ZygoteInit.main(ZygoteInit.java:1028)

--------------------------------

------------- System -----------

-----    ANDROID PHONE UNLOCK     -------

             ROOT = enable.            /).   false
             ROOT= Fix.         /). false
             ROOT=false. /). null

ROOT Checker=}false {.  ROOT Checker=}false {. ROOT Checker=}false {. ROOT Checker=}false {.  ROOT Checker=}false {

BUILD Code: 2023040901
SDK Code: 33 (Android 13 Tiramisu)
HASH Code: 19243045621 (10)
HEAP: Used memory: 9MB. Available memory: 503MB. Heap size: 512MB.


--------------------------------

------------- Build ------------

--------------------------------

BOARD: pissarro
BOOTLOADER: unknown
BRAND: Redmi
CPU_ABI: arm64-v8a
CPU_ABI2: 
DEVICE: pissarro
DISPLAY: TP1A.220624.014
FINGERPRINT: Redmi/pissarro/pissarro:13/TP1A.220624.014/V14.0.2.0.TKTINXM:user/release-keys
HARDWARE: mt6877
HOST: pangu-build-component-system-92621-943rc-ctrhp-v7mv8
ID: TP1A.220624.014
IS_DEBUGGABLE: false
IS_EMULATOR: false
IS_MIUI: true
MANUFACTURER: Xiaomi
MODEL: 21091116C
ODM_SKU: pissarro
PERMISSIONS_REVIEW_REQUIRED: true
PRODUCT: pissarro
RADIO: unknown
SERIAL: unknown
SKU: unknown
SOC_MANUFACTURER: Mediatek
SOC_MODEL: MT6877V/TZA
SUPPORTED_32_BIT_ABIS: armeabi-v7a, armeabi
SUPPORTED_64_BIT_ABIS: arm64-v8a
SUPPORTED_ABIS: arm64-v8a, armeabi-v7a, armeabi
TAGS: release-keys
TIME: 1680175478000
TYPE: user
UNKNOWN: unknown
USER: builder
----------------------------------



