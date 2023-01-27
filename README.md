# UnityDevConsole
Developer console for Unity game engine.

## Description

Allows executing console command in runtime.

![](https://gyazo.com/83f2c1207be51770b07bd21b5b1462ac.gif)

## Installation
Use [UPM](https://docs.unity3d.com/Manual/upm-ui.html) to install the package via the following git URL: 

```
https://github.com/dkoleev/UnityDevConsole.git
```

![](https://gyazo.com/8c8fc97345fc64f53d62814cce571974.gif)

## How to setup
  * For default [Input Manager](https://docs.unity3d.com/Manual/class-InputManager.html) drag `DevConsole_InputManager` prefab to scene.
  
  * For [New Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.5/manual/index.html) - drag `DevConsole_NewInputSystem` prefab to scene.
  
  * Add `ENABLE_DEV_CONSOLE` define to `ProjectSettings -> Scripting Define Symbols`.
  
  ![image](https://user-images.githubusercontent.com/54948242/215025591-3be1c75e-9f0b-44ff-8eda-1f53fda5e3f0.png)
  
  > You can add `ENABLE_DEV_CONSOLE` for dev build and remove it for release build in your build pipline.


 ## How to use
  * Create `public static` method.
  * Add attribute `DevConsoleCommand` for this method.
  * Specify the command name in the attribute parameter `commandName`.
  
  ```C#
      [DevConsoleCommand("sum")]
      public static void Sum(int arg1, int arg2) {
          Debug.Log(arg1 + arg2);
      }
  ```
  
  ![image](https://user-images.githubusercontent.com/54948242/214891842-0fe805e5-7200-44d6-b079-7a6d9c3b0ef0.png)
