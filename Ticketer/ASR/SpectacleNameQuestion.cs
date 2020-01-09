using Microsoft.Speech.Recognition;
using System.Collections.Generic;
using Ticketer.Database.Repositories;
using Ticketer.MyProperties;

namespace Ticketer.ASR
{
    class SpectacleNameQuestion : ASRAbstract
    {
        private SpeechRecognitionEngine pSRE;
        private Repository repository;
        private List<string> spectacles;

        public SpectacleNameQuestion()
        {
            pSRE = new SpeechRecognitionEngine(MyCultureInfo.PolishCulture);
            pSRE.SetInputToDefaultAudioDevice();
            repository = new Repository();
            spectacles = repository.GetSpectacles();
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

            foreach(string spectacle in spectacles)
            {
                choices.Add(spectacle);
            }
            grammarBuilder.Append(choices);
            Grammar grammar = new Grammar(grammarBuilder);

            engine.LoadGrammar(grammar);
        }
    }
}
