using Microsoft.Speech.Recognition;
using Ticketer.MyProperties;

namespace Ticketer.ASR
{
    class AfterReservationQuestion : ASRAbstract
    {
        private SpeechRecognitionEngine pSRE;

        public AfterReservationQuestion()
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
            choices.Add("Zakończ");
            choices.Add("Powtórz");
            grammarBuilder.Append(choices);
            Grammar grammar = new Grammar(grammarBuilder);

            engine.LoadGrammar(grammar);
        }
    }
}
