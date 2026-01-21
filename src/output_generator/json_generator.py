"""
JSON Report Generator
Generates structured JSON reports from analysis results
"""

import json
from typing import Dict
from datetime import datetime


class JSONReportGenerator:
    """Generator for JSON formatted analysis reports"""
    
    def generate_report(self, analysis_data: Dict, output_path: str = None) -> str:
        """
        Generate JSON report from analysis data
        
        Args:
            analysis_data: Analysis results dictionary
            output_path: Optional path to save the report
            
        Returns:
            JSON string of the report
        """
        report = {
            "metadata": {
                "report_type": "Crime Analysis Report",
                "generated_at": datetime.now().isoformat(),
                "version": "1.0.0"
            },
            "event_information": {
                "description": analysis_data.get("event_description", ""),
                "type": analysis_data.get("event_type", ""),
                "analysis_timestamp": analysis_data.get("timestamp", "")
            },
            "openai_analysis": self._format_openai_section(analysis_data),
            "legal_research": self._format_legal_research(analysis_data),
            "recommendations": self._format_recommendations(analysis_data),
            "status": analysis_data.get("status", "unknown")
        }
        
        json_output = json.dumps(report, ensure_ascii=False, indent=2)
        
        if output_path:
            with open(output_path, 'w', encoding='utf-8') as f:
                f.write(json_output)
        
        return json_output
    
    def _format_openai_section(self, data: Dict) -> Dict:
        """Format OpenAI analysis section"""
        openai_data = data.get("openai_analysis", {})
        
        return {
            "success": openai_data.get("success", False),
            "analysis": openai_data.get("analysis", ""),
            "model": openai_data.get("model", ""),
            "keywords_extracted": data.get("keywords", []),
            "tokens_used": openai_data.get("tokens_used", 0),
            "error": openai_data.get("error")
        }
    
    def _format_legal_research(self, data: Dict) -> Dict:
        """Format legal research section"""
        yargitay = data.get("yargitay_decisions", {})
        academic = data.get("academic_sources", {})
        correlation = data.get("correlation_analysis", {})
        
        return {
            "yargitay_decisions": {
                "success": yargitay.get("success", False),
                "findings": yargitay.get("decisions", ""),
                "keywords_used": yargitay.get("keywords", []),
                "error": yargitay.get("error")
            },
            "academic_sources": {
                "success": academic.get("success", False),
                "findings": academic.get("academic_sources", ""),
                "error": academic.get("error")
            },
            "correlation_analysis": {
                "success": correlation.get("success", False),
                "analysis": correlation.get("correlation_analysis", ""),
                "error": correlation.get("error")
            }
        }
    
    def _format_recommendations(self, data: Dict) -> Dict:
        """Format recommendations section"""
        recommendations_data = data.get("recommendations", {})
        
        return {
            "success": recommendations_data.get("success", False),
            "content": recommendations_data.get("recommendations", ""),
            "error": recommendations_data.get("error")
        }
