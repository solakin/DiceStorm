- Difficulties using stable diffusion (flux) to maintain a similar art aesthetic between different assets
- Finding a balance between spending time iterating through random seeds for a good gen or spending time refining the prompts to produce better gens in fewer iterations
- Better models produce quality images in fewer gens, but older models are much faster to iterate

- Qwen Image Q2 is heavily quantized so it's fast, but still very usable at about 5-10s per image


- Prompting the code
- Surprisingly adept at generating unity-usable plaintext files: uxml, uss, .cs, scriptableobjects, and more can generally be made and imported without a big problem.
- Manipulation of UI and game engine settings can be trickier: setting up scenes, configuring addressables, navigating assembly definition scoping, etc
- Blasted through a month's worth of free credits with Claude and Copilot, probably 3/4 or more of that usage was guiding it through bugs it had generated


- Responsibilities and ownership of data get thrown around all over the place by default

- Can get stuck in a loop trying to chase down compile errors, even if the error wasn't related to the specific thing it was asked to change at that point

- Telltale signs like and refactors leave lots of unused code lying around