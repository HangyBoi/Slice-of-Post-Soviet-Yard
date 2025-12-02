# Slice of Post-Soviet Yard: A Technical Art Portfolio

<img alt="image 6" src="https://github.com/user-attachments/assets/0961abfc-c7e1-4aa4-a013-785f0fef63ed" />

> "Shaders are the digital equivalent of a photographer's lens, lighting setup, and film stock all rolled into one." — *Project Reflection*

## 📖 The Story: The Digital Lens
As a programmer with a background in photography, I wanted to bridge the gap between **Engine Logic** and **Visual Art**. This project is not just a 3D environment; it is a custom rendering stack built in Unity URP.

The goal? To engineer a "Lens" that transforms standard 3D assets into a cohesive, atmospheric experience reminiscent of the PSX era, solving complex pipeline challenges along the way.

**[View the Full Portfolio on ArtStation](https://www.artstation.com/artwork/eRZ9kP)**

---

## 📚 Technical Deep Dive (Wiki)
I have documented the entire engineering process in the Wiki. Click a section to dive in:

### [📐 A. The Living World (Geometry)](https://github.com/HangyBoi/Slice-of-Post-Soviet-Yard/wiki/A.-The-Living-World:-Engineering-Reactive-Geometry)
* **Terrain Blending:** How I ditched Splat Maps for **Render Textures** to ground the grass.
* **Reactive Shaders:** Vertex Animation for wind and Depth-Buffer logic for water volume.
* **HLSL Integration:** Custom lighting functions inside Shader Graph.

### [📷 B. The Digital Lens (Pipeline)](https://github.com/HangyBoi/Slice-of-Post-Soviet-Yard/wiki/B.-The-Digital-Lens:-Pipeline-&-Post%E2%80%90Processing)
* **The Retro Stack:** Implementing Pixelation, Dithering, and Quantization as **Renderer Features**.
* **Pipeline Engineering:** Solving the "Fog vs. Transparency" injection point bug.
* **Atmosphere:** Creating volumetric-style fog using Height and Horizon fading.

### [🎨 C. Scene Assembly](https://github.com/HangyBoi/Slice-of-Post-Soviet-Yard/wiki/C.-Scene-Assembly:-Composition-&-Optimization)
* **Composition:** Building a "Post-Soviet" mood.
* **Optimization:** GPU Instancing and Render Texture economy.

---

## 🛠 Tech Stack
* **Engine:** Unity 6.2 (6000.2.13f1 - Supported) (URP)
* **Language:** HLSL (Custom Nodes), C# (Renderer Features)
* **Tools:** Shader Graph, Terrain Editor

## 👨‍💻 About Me
I am a Game Engineering student specializing in Technical Art. I focus on building tools and systems that empower visual fidelity without sacrificing performance.
