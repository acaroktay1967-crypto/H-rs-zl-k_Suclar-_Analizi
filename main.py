"""
Main Application Module
Entry point for the Crime Analysis System
"""

import argparse
import sys
from pathlib import Path

from src.analysis.crime_analysis import CrimeAnalysisService
from src.output_generator.json_generator import JSONReportGenerator
from src.output_generator.pdf_generator import PDFReportGenerator
from src.output_generator.openxml_generator import OpenXMLGenerator
from src.utils.config import Config


class CrimeAnalysisApp:
    """Main application for crime analysis"""
    
    def __init__(self):
        """Initialize the application"""
        self.config = Config()
        self.analysis_service = None
        self._init_services()
    
    def _init_services(self):
        """Initialize AI services"""
        openai_config = self.config.get_openai_config()
        deepseek_config = self.config.get_deepseek_config()
        
        self.analysis_service = CrimeAnalysisService(
            openai_key=openai_config['api_key'],
            deepseek_key=deepseek_config['api_key']
        )
    
    def analyze(self, event_description: str, event_type: str = "hırsızlık", 
                output_format: str = "json", output_path: str = None) -> dict:
        """
        Analyze a crime event and generate report
        
        Args:
            event_description: Description of the event
            event_type: Type of crime
            output_format: Format for output (json, pdf, docx)
            output_path: Path to save the report
            
        Returns:
            Analysis results dictionary
        """
        print(f"\n🔍 Analyzing {event_type} event...")
        print(f"Event: {event_description[:100]}...\n")
        
        # Perform analysis
        analysis_result = self.analysis_service.analyze_crime_event(
            event_description, event_type
        )
        
        # Generate output
        if output_path:
            self._generate_output(analysis_result, output_format, output_path)
        
        return analysis_result
    
    def quick_analysis(self, event_description: str) -> dict:
        """
        Perform quick analysis using only OpenAI
        
        Args:
            event_description: Description of the event
            
        Returns:
            Quick analysis results
        """
        print(f"\n⚡ Quick analysis...")
        print(f"Event: {event_description[:100]}...\n")
        
        return self.analysis_service.get_quick_analysis(event_description)
    
    def _generate_output(self, analysis_data: dict, output_format: str, output_path: str):
        """Generate output in specified format"""
        print(f"\n📄 Generating {output_format.upper()} report...")
        
        try:
            if output_format == 'json':
                generator = JSONReportGenerator()
                generator.generate_report(analysis_data, output_path)
                print(f"✅ JSON report saved to: {output_path}")
                
            elif output_format == 'pdf':
                generator = PDFReportGenerator()
                if generator.generate_report(analysis_data, output_path):
                    print(f"✅ PDF report saved to: {output_path}")
                else:
                    print(f"❌ Failed to generate PDF report")
                    
            elif output_format in ['docx', 'word', 'openxml']:
                generator = OpenXMLGenerator()
                if generator.generate_report(analysis_data, output_path):
                    print(f"✅ Word document saved to: {output_path}")
                else:
                    print(f"❌ Failed to generate Word document")
            else:
                print(f"⚠️  Unknown output format: {output_format}")
                
        except Exception as e:
            print(f"❌ Error generating output: {e}")


def main():
    """Main entry point"""
    parser = argparse.ArgumentParser(
        description='Hırsızlık Suçları Analiz Sistemi - Crime Analysis System'
    )
    
    parser.add_argument(
        'event_description',
        type=str,
        help='Description of the criminal event to analyze'
    )
    
    parser.add_argument(
        '-t', '--type',
        type=str,
        default='hırsızlık',
        help='Type of crime (default: hırsızlık)'
    )
    
    parser.add_argument(
        '-f', '--format',
        type=str,
        choices=['json', 'pdf', 'docx'],
        default='json',
        help='Output format (default: json)'
    )
    
    parser.add_argument(
        '-o', '--output',
        type=str,
        help='Output file path'
    )
    
    parser.add_argument(
        '-q', '--quick',
        action='store_true',
        help='Perform quick analysis (OpenAI only)'
    )
    
    args = parser.parse_args()
    
    try:
        app = CrimeAnalysisApp()
        
        # Validate configuration
        is_valid, warnings = app.config.validate_config()
        if warnings:
            print("⚠️  Configuration warnings:")
            for warning in warnings:
                print(f"  - {warning}")
            print()
        
        # Determine output path
        output_path = args.output
        if not output_path and args.format != 'json':
            output_dir = Path(app.config.get_output_config()['output_dir'])
            timestamp = Path.cwd().name
            extension = 'docx' if args.format == 'docx' else args.format
            output_path = str(output_dir / f"analysis_report.{extension}")
        
        # Perform analysis
        if args.quick:
            result = app.quick_analysis(args.event_description)
        else:
            result = app.analyze(
                args.event_description,
                args.type,
                args.format,
                output_path
            )
        
        # Print summary
        print("\n" + "="*50)
        print("Analysis Complete!")
        print("="*50)
        print(f"Status: {result.get('status', 'unknown')}")
        
        if result.get('keywords'):
            print(f"Keywords: {', '.join(result['keywords'])}")
        
    except KeyboardInterrupt:
        print("\n\n⚠️  Analysis interrupted by user")
        sys.exit(1)
    except Exception as e:
        print(f"\n❌ Error: {e}")
        sys.exit(1)


if __name__ == '__main__':
    main()
