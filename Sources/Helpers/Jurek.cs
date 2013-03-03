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

        public delegate void VoiceCommandDelegate(string cmd, float p);

        private VoiceCommandDelegate del;

        public Jurek()
        {
            talk = new SpeechSynthesizer();
            talk.SelectVoiceByHints(VoiceGender.Male);

            // Not available while using polish windows version without English package...
            Grammar g = new Grammar("D:/jurek.xml");
            listen = createSpeechEngine("EN-us");
            listen.RequestRecognizerUpdate();
            listen.LoadGrammar(g);
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

        public void StartListening(VoiceCommandDelegate callback)
        {
            Say("Słucham Cię");
            listen.RecognizeAsync(RecognizeMode.Multiple);
            del = callback;
        }

        private void  answer(object sender, SpeechRecognizedEventArgs e)
        {
            if (del != null)
                del(e.Result.Text, e.Result.Confidence);
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
