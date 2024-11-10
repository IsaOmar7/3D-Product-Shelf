# 3D-Product-Shelf
This project is a 3D interactive shelf that displays products fetched from an external server. Users can explore products with detailed visuals, audio feedback, and smooth animations.


## Instructions for Running the Project Locally

To run the project locally, follow these steps:

- Clone the Project:
Ensure the project folder, named **mocart-planets-shelf**, is downloaded on your computer.
Open Unity Hub and open the project you cloned

- Load the Main Scene:
In the Unity Editor, go to the Project window.
Navigate to Assets > Scenes and locate the Main Scene.
Double-click the Main Scene to open it.

- Run the Project:
With the Main Scene open, press the Play button at the top of the Unity Editor.
This will start the project, and you can begin interacting with the scene.







## External Libraries or Assets Utilized

This project uses the following external libraries and assets:

- TextMeshPro (TMP):
Used for high-quality text rendering in UI elements and 3D text displays. TMP provides clearer and more customizable text than Unity's default UI text.

- Unity Asset Store Assets:
Some visual and audio assets were sourced from the Unity Asset Store to enhance the project's environment, animations, and audio effects. These assets were selected to maintain visual consistency and improve user interaction quality.

in this section I used: 
   - Server Room asset
   - Planets of the solar system
   - Science Shelf

- Unity Standard Assets:
Unity Standard Assets used for standard elements like basic animations, character controllers, or effects were incorporated. **I used Third Person Controller and Target Following Camera**.

- Custom Audio Clips and Animations:
Custom audio clips and animations created specifically for this project were used, particularly for product interactions (such as clicking sounds or star animations and the planets names).

- AI voices:
I used AI voices to pronounce the name of the planet when its clicked







## Overview of Code Structure and Design Decisions

The project code is organized around key functionalities to support modularity, maintainability, and readability. 
Here’s an overview of the code structure and design decisions:

* Product Management (ProductManager.cs):
Responsible for fetching product data from an external server, displaying products on the shelf, and managing user interactions with product details this is the main file that handle the most of the actions in the scene.

    * Key decisions include:
Data Fetching: A coroutine handles data fetching from the server to avoid blocking the main thread.
Product Display Logic: Products are dynamically instantiated based on server data, enabling flexible updates without modifying the scene.
Quality Adjustment: Adjusts quality settings based on device memory, optimizing for desktop and mobile compatibility.


* Product Display and Interactions (ProductDisplay.cs):
Manages the appearance of each product, including animations (rotation, scaling) and click interactions that trigger detailed information on a UI canvas.
This script is a generic component attached to each product, allowing it to independently handle its own display, animations, and interactions. Each product instance behaves based on its unique data while sharing the same ProductDisplay script.
    * Design decisions:
    Rotating and Scaling: A continuous rotation effect and pulsing scale effect visually highlight products.
    Audio Feedback: Audio cues are triggered on product clicks, enhancing user interaction feedback.
  
* UI Management (CanvasManager.cs):
Handles transitions between the Welcome Canvas and the Product Details Canvas.
This component centralizes control over UI states, keeping UI-related functions separate from other logic, which improves code readability and reusability.

* User Interaction and Animation (CubeButton.cs, AstroAnim.cs):
CubeButton: Controls the main "start" interaction, triggering animations, canvas visibility, and product display when clicked.
AstroAnim: Applies continuous rotation to elements like stones, creating a dynamic environment.

    * Design decisions:
  Separated Animation Logic: Using dedicated scripts for animations (e.g., AstroAnim) allows animations to be applied across different objects easily.
  Generalized Components: Exposing properties like rotation speed or axis on animations enables reuse across various objects without modifying the script code.
