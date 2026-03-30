# CardGame
Slay the spire inspired game with focus on architecture and reusability. Don't mind the art.
![covertTOGif-ezgif com-video-to-gif-converter](https://github.com/user-attachments/assets/9c003ff3-9449-4842-a295-4083c44c6703)


In this project, I focused heavily on Separation of Concerns and Data-Driven Design to ensure the codebase remains clean and highly scalable. The core flexibility of the game lies in its robust ScriptableObject architecture. Cards are broken down into isolated CardEffect scripts (the abstract behavior) and EffectPayloads (the specific tuning variables, like damage or heal amounts). A single Card asset simply holds a list of these payloads. This guarantees that cards can easily trigger multiple effects sequentially, and it creates a highly designer-friendly environment where complex, multi-effect cards can be built and balanced entirely within the Unity Inspector without writing any new C# code.
