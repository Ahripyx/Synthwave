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
        // Properties
        private int score;
        public int Score => score;

        // Combo properties
        private int comboCount;
        private readonly int basePoints;
        private readonly int comboStep;

        // UI Elements
        private TextBlock scoreText;
        private TextBlock comboText;

        // Events
        public event Action<int> ScoreChanged;
        public event Action<int> ComboChanged;

        // Constructor
        public ScoreManager(int basePointsPerKill = 10, int comboStep = 5, int comboTimeoutMs = 2000)
        {
            this.basePoints = basePointsPerKill;
            this.comboStep = comboStep;
        }

        // Register a kill and update score/combo
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

        // Reset score and combo
        public void Reset()
        {
            score = 0;
            comboCount = 0;
            ScoreChanged?.Invoke(score);
            ComboChanged?.Invoke(comboCount);
            UpdateScoreText();
            UpdateComboText();
        }

        // Reset only the combo
        public void ResetCombo()
        {
            comboCount = 0;
            ComboChanged?.Invoke(comboCount);
            UpdateComboText();
            UpdateScoreText();
        }

        // Methods to register UI elements
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

        // Private methods to update UI
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
