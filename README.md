# Second-Order-Markov-Chain-Model
This is the GitHub repository for the coded portion of my Individual project for college. My project's aim is to create a 2nd Order MCM, train it on the Bach Chorales, and analyse its compositions, to determine whether it can produce harmonically convincing music. 
Please note that the initial timestamps for my code may not be wholly accurate as I had completed a portion of it before creating this repository. My project writeup is also linked to this repos, and is in the same situation.

## CHANGELOG
This will act as a progress report of what changes and improvements I make to the project as time progresses. Timestamps will be applied where possible, however I did not record these when I first started.
- Untracked, but chronological:
  - Created Word Document for project writeup, and introduced my project
  - Added References section and updated my sources for any content on the theoretical side of my project.
  - Created Project on vs2022: C# Console Template, using .NET 8.0
  - installed WetDryMidi NuGet package
  - Created Models.cs, with public record structs for the voices (SATB) "NoteTuple" and 2-state key for the MCM
  - Created MarkovChain.cs, added a dictionary for the voices and rng
  - In MarkovChain.cs, added "public void Train()" reading in the corpus using "IEnumerable<List<NoteTuple>> corpus", building a transition frequency table using sequence data. It filters out sequences with <3 elements and iterates through each valid sequence using a sliding window. It constructs a keystate with 2 consecutive notes and records the note that follows. It then adds the target note to the frequency table for that specific 2 note context in the dictionary.
  - In MarkovChain.cs, added "List<NoteTuple> Generate()" - this generates a sequence of notes starting from 2 seeds and using transitions from its training. 
  - Extended MarkovChain.cs "List<NoteTuple> Generate()" to feature a dead-end guard which ends generation if it reaches a pair that never appeared in training
- Tracked and timestamped
  - 25/08/2026:
    - Created MidiProcessor.cs and initialised NuGet Package WetDryMidi with the necessary "using ..." commands
    - In MidiProcessor.cs, created "List<NoteTuple> LoadChorale()", parsing a midi chorale file and extracting a sequence of 4-part chord tuples by grouping simultaneous notes and maps directly to SATB voices.
    - Added excess/lack of pitch handling to "List<NoteTuple> LoadChorale()" in MidiProcessor.cs so if more than 4 notes are playing simultaneously it takes the 4 highest. Similarly, any section with fewer than 4 sim. notes are dropped to make sure the tuple has uniform dimensions.
    - In MidiProcessor.cs, created "void ExportToMidi()", which converts the sequence of harmony tuples back into raw midi. Set stepDuration to 480 ticks (equal to crotchet) and creates a new note event at velocity 90 and a corresponding NoteOFfEvent w/ velocity 0. Appends all midi events (on+off) into the TrackChunk.
  - 29/08/2026:
    - Edited ExportToMidi() in MidiProcessor.cs to explicitly cast integer pitch values to seven bits to comply with standard MIDI pitch range
    - Updated class Program.cs to complete void Main(), which sets up end-to-end pipeline: batch dataset loading, model training, probabilistic generation from seed, MIDI file export.

## FUTURE UPDATES / TO BE ADDED
At the start of the project, I wanted to be able to compare how a 1st and 2nd order MCMs' compositions would differ, but didn't commit to doing it as I was unsure of the time and effort this 2nd order model will take. After having (mostly) completed it (aside from specific debugging relating to the scope of NoteTuple), I think that I will opt to add a user option between 1st and 2nd order generation. 3rd order generation may also be possible but I feel that it would diverge too much from my original project topic question, which is more focused on how convincing a MCM composed piece could be as opposed to how this can scale with different orders of MCM, however I may go back on this decision and implement a 3rd Order MCM
Another possibility I briefly talked about was a potential expansion into other genres of music - if I can find a good number of appropriate MIDI files for training, there is no good reason I don't at least attempt this.
Finally, there is the obvious need for debugging, because as of writing this, the program won't run due to syntax errors. I think I know how to fix them, however I am leaving that for a later date
