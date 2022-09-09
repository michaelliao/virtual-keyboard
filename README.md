# virtual-keyboard
Virtual keyboard is a simulated keyboard with RS-232 serial port support.

```
   ╔════════════════════╗
   ║                    ║
   ║   ┌────────────┐   ║
   ║   │  Virtual   │   ║
   ║   │  Keyboard  │   ║
   ║   │Application │   ║
   ║   └────────────┘   ║
   ╚════╦══════════╦════╝
  ╔═════╩══════════╩═════╗     ┌───────┐
  ║  ■ ■ ■      ┌──────┐ ║     │FPGA / │
┌─╣             └──────┘ ╠────█│8051 / │
│ ╚══════════════════════╝     │Arduino│
│   ┌──────────────────┐       └───────┘
└───┤ □□□□□□□□□□□□ □□□ │
    │ □□□□□□□□□□□□  □  │
    │ □ ======== □ □□□ │
    └──────────────────┘
```

### Download

Download VirtualKeyboard.exe for Windows [x64 version](https://github.com/michaelliao/virtual-keyboard/raw/master/Download/x64/VirtualKeyboard.exe) or [x86 version](https://github.com/michaelliao/virtual-keyboard/raw/master/Download/x86/VirtualKeyboard.exe).

If you run `VirtualKeyboard.exe` with an error like this:

![app-launch-failed](https://github.com/michaelliao/virtual-keyboard/blob/master/Download/resources/app-launch-failed.png?raw=true)

Click `Yes` to download .Net 6 Runtime from browser to run desktop apps:

![download-runtime](https://github.com/michaelliao/virtual-keyboard/blob/master/Download/resources/download-runtime.png?raw=true)

You should choose `Windows` - `Download x64` or `Download x86`.
