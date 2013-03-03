using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Windows.Forms;

namespace Helpers
{
    /// <summary>
    /// Text to Speech (and vice versa?) by Jurek :)
    /// </summary>
    public class Jurek
    {
        SpeechSynthesizer talk;
        SpeechRecognitionEngine listen;
        List<string> words = new List<string>()
        {
            "yoorek", "start", "stop", "prepare"
        };

        public delegate void VoiceCommandDelegate(string cmd, float p);

        private VoiceCommandDelegate del;

        public Jurek(string grammar = "")
        {
            talk = new SpeechSynthesizer();
            talk.SelectVoiceByHints(VoiceGender.Male);

            // Not available while using polish windows version without English package...
            listen = createSpeechEngine("EN-us");
            listen.RequestRecognizerUpdate();

            initGrammar(grammar);

            listen.SetInputToDefaultAudioDevice();
            listen.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(answer);
        }

        private SpeechRecognitionEngine createSpeechEngine(string locale)
        {
            int available = 0;
            foreach (RecognizerInfo config in SpeechRecognitionEngine.InstalledRecognizers())
            {
                ++available;
                if (config.Culture.ToString() == locale)
                    return new SpeechRecognitionEngine(locale);
            }

            if (available == 0) throw new Exception("No speech recognition engines available. Install them.");

            return new SpeechRecognitionEngine();
        }

        private void initGrammar(string grammar)
        {
            Grammar g = null;

            if (grammar == "")
                g = buildGrammar();
            else
            {
                try
                {
                    g = new Grammar(grammar);
                }
                catch (Exception)
                {
                    g = buildGrammar();
                }
            }

            listen.LoadGrammar(g);
        }

        private Grammar buildGrammar()
        {
            Choices c = new Choices();
            c.Add(words.ToArray());
            GrammarBuilder grammarBuilder = new GrammarBuilder(c);
            grammarBuilder.Culture = new System.Globalization.CultureInfo("EN-us");
            return new Grammar(grammarBuilder);
        }

        public void StartListening(VoiceCommandDelegate callback)
        {
            listen.RecognizeAsync(RecognizeMode.Multiple);
            del = callback;
        }

        private void  answer(object sender, SpeechRecognizedEventArgs e)
        {
            if (del != null)
            {
                foreach (var word in e.Result.Words)
                {
                    if (word.Confidence >= 0.7)
                        del(word.Text, word.Confidence);
                }
                
            }
        }

        public void Say(string what)
        {
            talk.SpeakAsync(what);
        }

        public void SayAndWait(string what)
        {
            talk.Speak(what);
        }
    }
}
