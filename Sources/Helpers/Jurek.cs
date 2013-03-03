using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using System.Speech.Recognition;

namespace Helpers
{
    /// <summary>
    /// Text to Speech (and vice versa?) by Jurek :)
    /// </summary>
    public class Jurek
    {
        SpeechSynthesizer talk;
        SpeechRecognitionEngine listen;

        public Jurek()
        {
            talk = new SpeechSynthesizer();
            talk.SelectVoiceByHints(VoiceGender.Male);

            // Not available while using polish windows version without englisch package...
            //Grammar g = new Grammar("D:/jurek.xml");
            //listen = new SpeechRecognitionEngine();
            //listen.RequestRecognizerUpdate();
            //listen.LoadGrammar(g);
            //listen.SetInputToDefaultAudioDevice();
            //listen.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(answer);
        }

        public void StartListening()
        {
            Say("Słucham Cię");
            listen.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void  answer(object sender, SpeechRecognizedEventArgs e)
        {
 	        talk.SpeakAsync("Zrozumiałem. Powiedziałeś: " + e.Result.Text + ", prawda?");
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
