"""
Example usage of the Crime Analysis System
"""

from src.analysis.crime_analysis import CrimeAnalysisService
from src.output_generator.json_generator import JSONReportGenerator
from src.output_generator.pdf_generator import PDFReportGenerator
from src.output_generator.openxml_generator import OpenXMLGenerator


def example_basic_analysis():
    """Example: Basic crime analysis"""
    print("="*60)
    print("Example 1: Basic Crime Analysis")
    print("="*60)
    
    # Initialize service
    service = CrimeAnalysisService()
    
    # Analyze an event
    event = """
    Şüpheli, gece saatlerinde pencereyi kırarak eve giriş yapmış ve içerideki 
    değerli eşyaları (altın takılar, nakit para ve elektronik cihazlar) 
    çalarak kaçmıştır. Güvenlik kamerasında görüntüsü tespit edilmiştir.
    """
    
    result = service.analyze_crime_event(event, "nitelikli hırsızlık")
    
    print(f"\nAnalysis Status: {result['status']}")
    print(f"Keywords: {result.get('keywords', [])}")
    
    if result['openai_analysis'] and result['openai_analysis'].get('success'):
        print("\n--- OpenAI Analysis ---")
        print(result['openai_analysis']['analysis'][:300] + "...")


def example_quick_analysis():
    """Example: Quick analysis using only OpenAI"""
    print("\n" + "="*60)
    print("Example 2: Quick Analysis")
    print("="*60)
    
    service = CrimeAnalysisService()
    
    event = "Mağazadan cep telefonu çalan kişi yakalanmıştır."
    
    result = service.get_quick_analysis(event)
    
    if result.get('success'):
        print("\nQuick Analysis Result:")
        print(result['analysis'][:200] + "...")


def example_json_output():
    """Example: Generate JSON report"""
    print("\n" + "="*60)
    print("Example 3: JSON Report Generation")
    print("="*60)
    
    service = CrimeAnalysisService()
    
    event = "Araç içerisinden laptop çalınması olayı"
    result = service.get_quick_analysis(event)
    
    # Generate JSON report
    generator = JSONReportGenerator()
    
    # Prepare data in expected format
    report_data = {
        "event_description": event,
        "event_type": "hırsızlık",
        "timestamp": "2024-01-01T10:00:00",
        "openai_analysis": result,
        "status": "success" if result.get('success') else "failed"
    }
    
    json_output = generator.generate_report(report_data)
    print("\nJSON Report Generated:")
    print(json_output[:300] + "...")


def example_keyword_search():
    """Example: Search using keywords"""
    print("\n" + "="*60)
    print("Example 4: Case Law Search")
    print("="*60)
    
    service = CrimeAnalysisService()
    
    keywords = ["nitelikli hırsızlık", "gece vakti", "konuta giriş"]
    result = service.search_case_law(keywords, "hırsızlık")
    
    if result.get('success'):
        print("\nCase Law Search Results:")
        print(result['decisions'][:300] + "...")
    else:
        print(f"\nSearch failed: {result.get('error', 'Unknown error')}")


def main():
    """Run all examples"""
    print("\n🚀 Crime Analysis System - Examples\n")
    
    try:
        # Example 1: Basic analysis
        example_basic_analysis()
        
        # Example 2: Quick analysis
        example_quick_analysis()
        
        # Example 3: JSON output
        example_json_output()
        
        # Example 4: Keyword search
        example_keyword_search()
        
        print("\n✅ All examples completed!")
        
    except ValueError as e:
        print(f"\n⚠️  Configuration Error: {e}")
        print("\nPlease set the following environment variables:")
        print("  - OPENAI_API_KEY: Your OpenAI API key")
        print("  - DEEPSEEK_API_KEY: Your DeepSeek API key")
    except Exception as e:
        print(f"\n❌ Error running examples: {e}")


if __name__ == '__main__':
    main()
