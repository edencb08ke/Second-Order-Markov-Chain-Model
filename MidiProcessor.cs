using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Standards;

namespace Second_Order_Markov_Chain_Model;
    public class MidiProcessor
    {
        public List<NoteTuple> LoadChorale(string filePath)
        {
            var midiFile = MidiFile.Read(filePath);
            var notes = midiFile.GetNotes();

            // group notes by start time to form chords
            var groupedNotes = notes
                .GroupBy(n => n.Time)
                .OrderBy(g => g.Key);

            var sequence = new List<NoteTuple>();
            foreach (var group in groupedNotes)
            {
                var sortedPitches = group
                    .Select(n => (int)n.NoteNumber)
                    .OrderByDescending(p => p)
                    .ToList();
                // verify all 4 voices
                if (sortedPitches.Count >= 4)
                {
                    sequence.Add(new NoteTuple(
                        sortedPitches[0], // S
                        sortedPitches[1], // A
                        sortedPitches[2], // T
                        sortedPitches[3] // B
                        ));
                }
            }
            return sequence;
        }
        public void ExportToMidi(List<NoteTuple> sequence, string ouputPath, int tempoBPM = 80)
        {
            var midiFile = new MidiFile();
            var trackChunk = new TrackChunk();

            // set crotchet step duration
            long stepDuration = 480; // ticks per crotchet

            for (int i = 0; i < sequence.Count; i++)
            {
                long time = i * stepDuration;
                var chord = sequence[i];

                int[] pitches = { chord.Soprano, chord.Alto, chord.Tenor, chord.Bass };
                foreach (var pitch in pitches)
                {
                    var noteOn = new NoteOnEvent((SevenBitNumber)pitch, (SevenBitNumber)90) { Time = time };
                    var noteOff = new NoteOffEvent((SevenBitNumber)pitch, (SevenBitNumber)0) { Time = time + stepDuration };

                    trackChunk.Events.Add(noteOn);
                    trackChunk.Events.Add(noteOff);
                }
            }
            midiFile.Chunks.Add(trackChunk);
            midiFile.Write(ouputPath, true);
        }
    }
