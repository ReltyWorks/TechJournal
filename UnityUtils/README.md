# UnityUtils

**UnityUtils** is a collection of reusable C# scripts and utilities designed to streamline Unity development. This repository contains code that I have originally created or heavily modified to optimize workflows and enhance productivity in various projects.


### 1. BindBehaviour
**`BindBehaviour`** is an automatic dependency injection system that links `private` fields to GameObjects or Components using custom attributes and Reflection.
* **Features:** automatically binds fields by matching them with Hierarchy names using a specific naming convention (Prefix + PascalCase).
* **Usage:** Inherit `BindBehaviour` and attach attributes like `[Bind]`, `[BindList]`, or `[BindRoot]` to your fields. It eliminates repetitive `GetComponent` or `Find` calls in `Awake`.

### 2. SlidingWindow
**`SlidingWindow`** is a UI utility that smoothly animates a `RectTransform` between open and closed positions based on mouse interactions.
* **Features:** Supports intuitive interactions including "Hover to Open," "Exit to Close," and a "Click to Fix/Unfix" toggle to keep the window open.
* **Usage:** Attach this script to a UI object, configure the target positions (`OpenPos`, `ClosePos`) and speed in the Inspector, and it works immediately without extra setup.