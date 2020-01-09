using Microsoft.Speech.Recognition;
using System;
using Ticketer.MyProperties;

namespace Ticketer.ASR
{
    class TicketTypeQuestion : ASRAbstract
    {
        private SpeechRecognitionEngine pSRE;

        public TicketTypeQuestion()
        {
            pSRE = new SpeechRecognitionEngine(MyCultureInfo.PolishCulture);
            pSRE.SetInputToDefaultAudioDevice();
            PrepareGrammar(pSRE);
        }

        public override string GetAnswer()
        {
            return AnswerReceiver.GetAnswer(pSRE);
        }

        protected override void PrepareGrammar(SpeechRecognitionEngine engine)
        {
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            Choices choices = new Choices();
            choices.Add("Dziecięcy");
            choices.Add("Ulgowy");
            choices.Add("Normalny");
            grammarBuilder.Append(choices);
            Grammar grammar = new Grammar(grammarBuilder);

            engine.LoadGrammar(grammar);
        }
    }
}
