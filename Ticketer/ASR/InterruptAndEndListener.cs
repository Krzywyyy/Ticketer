using Microsoft.Speech.Recognition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketer.ASR.Validators;
using Ticketer.MyProperties;

namespace Ticketer.ASR
{
    class InterruptAndEndListener : ASRAbstract
    {
        private SpeechRecognitionEngine pSRE;

        public InterruptAndEndListener()
        {
            pSRE = new SpeechRecognitionEngine(MyCultureInfo.PolishCulture);
            pSRE.SetInputToDefaultAudioDevice();
            PrepareGrammar(pSRE);
        }

        public override string GetAnswer()
        {
            return AnswerReceiver.GetInterruption(pSRE);
        }

        protected override void PrepareGrammar(SpeechRecognitionEngine engine)
        {
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            Choices choices = new Choices();
            choices.Add("Przerwij");
            choices.Add("Zakończ");
            grammarBuilder.Append(choices);
            
            Grammar grammar = new Grammar(grammarBuilder);

            engine.LoadGrammar(grammar);
        }
    }
}
