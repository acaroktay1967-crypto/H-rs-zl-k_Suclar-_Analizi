"""
Basic tests for the Crime Analysis System
"""

import unittest
from unittest.mock import Mock, patch, MagicMock
import sys
import os

# Add parent directory to path
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))

from src.analysis.crime_analysis import CrimeAnalysisService
from src.output_generator.json_generator import JSONReportGenerator
from src.utils.config import Config


class TestConfiguration(unittest.TestCase):
    """Test configuration management"""
    
    def test_config_initialization(self):
        """Test that config can be initialized"""
        config = Config()
        self.assertIsNotNone(config.settings)
    
    def test_config_structure(self):
        """Test config has required sections"""
        config = Config()
        self.assertIn('openai', config.settings)
        self.assertIn('deepseek', config.settings)
        self.assertIn('output', config.settings)


class TestJSONGenerator(unittest.TestCase):
    """Test JSON report generator"""
    
    def test_json_generation(self):
        """Test basic JSON report generation"""
        generator = JSONReportGenerator()
        
        sample_data = {
            "event_description": "Test event",
            "event_type": "hırsızlık",
            "timestamp": "2024-01-01T10:00:00",
            "status": "success",
            "openai_analysis": {
                "success": True,
                "analysis": "Test analysis",
                "model": "test-model"
            },
            "keywords": ["test", "keywords"]
        }
        
        json_output = generator.generate_report(sample_data)
        self.assertIsNotNone(json_output)
        self.assertIn("metadata", json_output)
        self.assertIn("event_information", json_output)
    
    def test_json_formatting(self):
        """Test JSON report formatting sections"""
        generator = JSONReportGenerator()
        
        sample_data = {
            "event_description": "Test",
            "event_type": "test",
            "timestamp": "2024-01-01",
            "status": "success"
        }
        
        result = generator._format_openai_section(sample_data)
        self.assertIsInstance(result, dict)
        self.assertIn("success", result)


class TestCrimeAnalysisService(unittest.TestCase):
    """Test crime analysis service"""
    
    @patch.dict(os.environ, {'OPENAI_API_KEY': 'test_key', 'DEEPSEEK_API_KEY': 'test_key'})
    def test_service_initialization(self):
        """Test service can be initialized with API keys"""
        service = CrimeAnalysisService(
            openai_key='test_openai_key',
            deepseek_key='test_deepseek_key'
        )
        self.assertIsNotNone(service.openai_service)
        self.assertIsNotNone(service.deepseek_service)
    
    def test_service_without_keys(self):
        """Test service initialization without API keys"""
        # Clear environment variables
        with patch.dict(os.environ, {}, clear=True):
            service = CrimeAnalysisService()
            # Service should initialize but AI services may be None
            self.assertIsNotNone(service)


class TestOpenAIService(unittest.TestCase):
    """Test OpenAI service"""
    
    def test_service_requires_key(self):
        """Test that OpenAI service requires an API key"""
        from src.ai_integration.openai_service import OpenAIService
        
        with patch.dict(os.environ, {}, clear=True):
            with self.assertRaises(ValueError):
                OpenAIService()
    
    @patch('openai.resources.completions.Completions.create')
    def test_analyze_event_mock(self, mock_create):
        """Test event analysis with mocked OpenAI response"""
        from src.ai_integration.openai_service import OpenAIService
        
        # Mock OpenAI response
        mock_response = Mock()
        mock_response.choices = [Mock()]
        mock_response.choices[0].text = "Test analysis result"
        mock_response.usage.total_tokens = 100
        mock_create.return_value = mock_response
        
        service = OpenAIService(api_key='test_key')
        result = service.analyze_event("Test event", "test")
        
        self.assertTrue(result['success'])
        self.assertIn('analysis', result)


class TestDeepSeekService(unittest.TestCase):
    """Test DeepSeek service"""
    
    def test_service_requires_key(self):
        """Test that DeepSeek service requires an API key"""
        from src.ai_integration.deepseek_service import DeepSeekService
        
        with patch.dict(os.environ, {}, clear=True):
            with self.assertRaises(ValueError):
                DeepSeekService()
    
    @patch('requests.post')
    def test_search_yargitay_decisions_mock(self, mock_post):
        """Test Yargıtay decision search with mocked response"""
        from src.ai_integration.deepseek_service import DeepSeekService
        
        # Mock successful response
        mock_response = Mock()
        mock_response.status_code = 200
        mock_response.json.return_value = {
            "choices": [{
                "message": {
                    "content": "Test decision results"
                }
            }]
        }
        mock_post.return_value = mock_response
        
        service = DeepSeekService(api_key='test_key')
        result = service.search_yargitay_decisions(['test'], 'test')
        
        self.assertTrue(result['success'])
        self.assertIn('decisions', result)


def run_tests():
    """Run all tests"""
    loader = unittest.TestLoader()
    suite = unittest.TestSuite()
    
    # Add all test classes
    suite.addTests(loader.loadTestsFromTestCase(TestConfiguration))
    suite.addTests(loader.loadTestsFromTestCase(TestJSONGenerator))
    suite.addTests(loader.loadTestsFromTestCase(TestCrimeAnalysisService))
    suite.addTests(loader.loadTestsFromTestCase(TestOpenAIService))
    suite.addTests(loader.loadTestsFromTestCase(TestDeepSeekService))
    
    runner = unittest.TextTestRunner(verbosity=2)
    result = runner.run(suite)
    
    return result.wasSuccessful()


if __name__ == '__main__':
    success = run_tests()
    sys.exit(0 if success else 1)
