"""
Crime Analysis Service
Combines OpenAI and DeepSeek services for comprehensive legal analysis
"""

from typing import Dict, Optional
from datetime import datetime
from ..ai_integration.openai_service import OpenAIService
from ..ai_integration.deepseek_service import DeepSeekService


class CrimeAnalysisService:
    """Service for comprehensive crime analysis using multiple AI services"""
    
    def __init__(self, openai_key: Optional[str] = None, deepseek_key: Optional[str] = None):
        """
        Initialize Crime Analysis Service
        
        Args:
            openai_key: OpenAI API key (optional, can use env variable)
            deepseek_key: DeepSeek API key (optional, can use env variable)
        """
        try:
            self.openai_service = OpenAIService(api_key=openai_key)
        except ValueError as e:
            print(f"Warning: OpenAI service not available - {e}")
            self.openai_service = None
        
        try:
            self.deepseek_service = DeepSeekService(api_key=deepseek_key)
        except ValueError as e:
            print(f"Warning: DeepSeek service not available - {e}")
            self.deepseek_service = None
    
    def analyze_crime_event(self, event_description: str, event_type: str = "hırsızlık") -> Dict:
        """
        Perform comprehensive analysis of a crime event
        
        Args:
            event_description: Description of the criminal event
            event_type: Type of crime (default: hırsızlık/theft)
            
        Returns:
            Dictionary containing complete analysis results
        """
        analysis_result = {
            "timestamp": datetime.now().isoformat(),
            "event_description": event_description,
            "event_type": event_type,
            "openai_analysis": None,
            "recommendations": None,
            "yargitay_decisions": None,
            "academic_sources": None,
            "correlation_analysis": None,
            "keywords": [],
            "status": "incomplete"
        }
        
        # Step 1: OpenAI Event Analysis
        if self.openai_service:
            print("Performing OpenAI analysis...")
            openai_result = self.openai_service.analyze_event(event_description, event_type)
            analysis_result["openai_analysis"] = openai_result
            
            if openai_result.get("success"):
                # Extract keywords for case law search
                keywords = self.openai_service.extract_keywords(
                    openai_result.get("analysis", "")
                )
                analysis_result["keywords"] = keywords
                
                # Generate recommendations
                recommendations = self.openai_service.generate_recommendations(
                    event_description, openai_result
                )
                analysis_result["recommendations"] = recommendations
        
        # Step 2: DeepSeek Yargıtay Decisions Search
        if self.deepseek_service and analysis_result["keywords"]:
            print("Searching Yargıtay decisions...")
            decisions_result = self.deepseek_service.search_yargitay_decisions(
                analysis_result["keywords"], event_type
            )
            analysis_result["yargitay_decisions"] = decisions_result
            
            # Correlate decisions with event
            if decisions_result.get("success"):
                correlation = self.deepseek_service.correlate_decisions(
                    event_description,
                    decisions_result.get("decisions", "")
                )
                analysis_result["correlation_analysis"] = correlation
        
        # Step 3: Academic Sources Search
        if self.deepseek_service and analysis_result["keywords"]:
            print("Searching academic sources...")
            academic_result = self.deepseek_service.search_academic_sources(
                event_description,
                analysis_result["keywords"]
            )
            analysis_result["academic_sources"] = academic_result
        
        # Determine overall status
        if (analysis_result["openai_analysis"] and 
            analysis_result["openai_analysis"].get("success")):
            analysis_result["status"] = "success"
        else:
            analysis_result["status"] = "partial"
        
        return analysis_result
    
    def get_quick_analysis(self, event_description: str) -> Dict:
        """
        Get a quick analysis using only OpenAI
        
        Args:
            event_description: Description of the criminal event
            
        Returns:
            Dictionary containing quick analysis results
        """
        if not self.openai_service:
            return {
                "success": False,
                "error": "OpenAI service not available"
            }
        
        result = self.openai_service.analyze_event(event_description)
        
        if result.get("success"):
            recommendations = self.openai_service.generate_recommendations(
                event_description, result
            )
            result["recommendations"] = recommendations
        
        return result
    
    def search_case_law(self, keywords: list, event_type: str = "hırsızlık") -> Dict:
        """
        Search for case law using DeepSeek
        
        Args:
            keywords: List of keywords to search
            event_type: Type of crime
            
        Returns:
            Dictionary containing case law search results
        """
        if not self.deepseek_service:
            return {
                "success": False,
                "error": "DeepSeek service not available"
            }
        
        return self.deepseek_service.search_yargitay_decisions(keywords, event_type)
