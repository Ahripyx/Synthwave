using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GameLibrary.Managers
{
    public class ScoreManager
    {

        private int score;
        public int Score => score;

        private int comboCount;
        private readonly int basePoints;
        private readonly int comboStep;

        private TextBlock scoreText;
        private TextBlock comboText;

        public event Action<int> ScoreChanged;
        public event Action<int> ComboChanged;

        public ScoreManager(int basePointsPerKill = 10, int comboStep = 5, int comboTimeoutMs = 2000)
        {
            this.basePoints = basePointsPerKill;
            this.comboStep = comboStep;
        }

        public void RegisterKill()
        {
            comboCount++;

            int multiplier = 1 + (comboCount / comboStep);
            int points = basePoints * multiplier;
            score += points;

            ScoreChanged?.Invoke(score);
            ComboChanged?.Invoke(comboCount);
            UpdateScoreText();
            UpdateComboText();
        }

        public void Reset()
        {
            score = 0;
            comboCount = 0;
            ScoreChanged?.Invoke(score);
            ComboChanged?.Invoke(comboCount);
            UpdateScoreText();
            UpdateComboText();
        }

        // Public: reset only the combo (call when player takes damage)
        public void ResetCombo()
        {
            comboCount = 0;
            ComboChanged?.Invoke(comboCount);
            UpdateComboText();
            UpdateScoreText();
        }

        public void RegisterScoreTextBlock(TextBlock tb)
        {
            scoreText = tb;
            UpdateScoreText();
        }

        public void RegisterComboTextBlock(TextBlock tb)
        {
            comboText = tb;
            comboText.Visibility = Visibility.Collapsed;
            comboText.Text = string.Empty;
        }

        private void UpdateScoreText()
        {
            if (scoreText == null) return;
            int multiplier = 1 + (comboCount / comboStep);
            if (multiplier > 1)
                scoreText.Text = $"Score: {score}";
            else
                scoreText.Text = $"Score: {score}";
        }

        private void UpdateComboText()
        {
            if (comboText == null) return;
            if (comboCount <= 1)
            {
                comboText.Text = string.Empty;
                comboText.Visibility = Visibility.Collapsed;
            }
            else
            {
                int multiplier = 1 + (comboCount / comboStep);
                comboText.Text = $"Combo: x{multiplier}";
                comboText.Visibility = Visibility.Visible;
            }
        }
    }
}
