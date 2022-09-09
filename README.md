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

### Usage

Click `Power` button to open a serial port after launch the app:

![keyboard-initial](https://github.com/michaelliao/virtual-keyboard/blob/master/Download/resources/keyboard-initial.png?raw=true)

Select port name to open the serial port:

![open-serial-port](https://github.com/michaelliao/virtual-keyboard/blob/master/Download/resources/open-serial-port.png?raw=true)

You can click the key button to send keys:

![keyboard-send-keys](https://github.com/michaelliao/virtual-keyboard/blob/master/Download/resources/keyboard-send-keys.png?raw=true)

You can also type the keyboard to send keys directly, but make sure the Virtual Keyboard window is activated in order to receive the keyboard input.

The code sent to the serial port can be found in the status bar. For example, the key `Shift` + `A` produces codes `12 1C F0 1C F0 12`.
