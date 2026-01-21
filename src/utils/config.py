"""
Configuration Management
Handles loading and validation of API keys and settings
"""

import os
from typing import Dict, Optional
from pathlib import Path


class Config:
    """Configuration manager for the application"""
    
    def __init__(self, config_file: Optional[str] = None):
        """
        Initialize configuration
        
        Args:
            config_file: Optional path to configuration file
        """
        self.config_file = config_file
        self.settings = self._load_config()
    
    def _load_config(self) -> Dict:
        """Load configuration from environment variables or file"""
        config = {
            'openai': {
                'api_key': os.getenv('OPENAI_API_KEY'),
                'model': os.getenv('OPENAI_MODEL', 'text-davinci-003'),
                'max_tokens': int(os.getenv('OPENAI_MAX_TOKENS', '1500')),
                'temperature': float(os.getenv('OPENAI_TEMPERATURE', '0.7'))
            },
            'deepseek': {
                'api_key': os.getenv('DEEPSEEK_API_KEY'),
                'api_base': os.getenv('DEEPSEEK_API_BASE', 'https://api.deepseek.com/v1'),
                'timeout': int(os.getenv('DEEPSEEK_TIMEOUT', '30'))
            },
            'output': {
                'default_format': os.getenv('OUTPUT_FORMAT', 'json'),
                'output_dir': os.getenv('OUTPUT_DIR', './reports')
            }
        }
        
        # Create output directory if it doesn't exist
        output_dir = Path(config['output']['output_dir'])
        output_dir.mkdir(parents=True, exist_ok=True)
        
        return config
    
    def get_openai_config(self) -> Dict:
        """Get OpenAI configuration"""
        return self.settings['openai']
    
    def get_deepseek_config(self) -> Dict:
        """Get DeepSeek configuration"""
        return self.settings['deepseek']
    
    def get_output_config(self) -> Dict:
        """Get output configuration"""
        return self.settings['output']
    
    def validate_config(self) -> tuple[bool, list]:
        """
        Validate configuration
        
        Returns:
            Tuple of (is_valid, list_of_warnings)
        """
        warnings = []
        
        if not self.settings['openai']['api_key']:
            warnings.append("OpenAI API key not configured")
        
        if not self.settings['deepseek']['api_key']:
            warnings.append("DeepSeek API key not configured")
        
        is_valid = len(warnings) == 0
        
        return is_valid, warnings
