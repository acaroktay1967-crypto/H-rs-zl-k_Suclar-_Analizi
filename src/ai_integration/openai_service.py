"""
OpenAI API Integration Service
Provides event analysis and recommendation generation using OpenAI models
"""

import os
from typing import Dict, List, Optional
from openai import OpenAI


class OpenAIService:
    """Service for interacting with OpenAI API"""
    
    def __init__(self, api_key: Optional[str] = None, model: str = "text-davinci-003"):
        """
        Initialize OpenAI service
        
        Args:
            api_key: OpenAI API key. If None, reads from OPENAI_API_KEY env variable
            model: Model to use for completions (default: text-davinci-003)
        """
        self.api_key = api_key or os.getenv("OPENAI_API_KEY")
        if not self.api_key:
            raise ValueError("OpenAI API key must be provided or set in OPENAI_API_KEY environment variable")
        
        self.client = OpenAI(api_key=self.api_key)
        self.model = model
        
    def analyze_event(self, event_description: str, event_type: str = "hırsızlık") -> Dict:
        """
        Analyze a criminal event using OpenAI
        
        Args:
            event_description: Description of the criminal event
            event_type: Type of crime (default: hırsızlık/theft)
            
        Returns:
            Dictionary containing analysis results
        """
        prompt = f"""
        Aşağıdaki {event_type} olayını Türk Ceza Kanunu kapsamında analiz et:
        
        Olay: {event_description}
        
        Lütfen şu konularda detaylı analiz yap:
        1. Suçun unsurları ve nitelikli halleri
        2. İlgili TCK maddeleri
        3. Olası cezai yaptırımlar
        4. Savunma stratejileri
        5. İlgili Yargıtay içtihatları için anahtar kelimeler
        """
        
        try:
            response = self.client.completions.create(
                model=self.model,
                prompt=prompt,
                max_tokens=1500,
                temperature=0.7,
                top_p=1.0,
                frequency_penalty=0.0,
                presence_penalty=0.0
            )
            
            analysis_text = response.choices[0].text.strip()
            
            return {
                "success": True,
                "event_type": event_type,
                "event_description": event_description,
                "analysis": analysis_text,
                "model": self.model,
                "tokens_used": response.usage.total_tokens
            }
        except Exception as e:
            return {
                "success": False,
                "error": str(e),
                "event_description": event_description
            }
    
    def generate_recommendations(self, event_context: str, analysis_data: Optional[Dict] = None) -> Dict:
        """
        Generate legal recommendations based on event context
        
        Args:
            event_context: Context of the event
            analysis_data: Optional previous analysis data
            
        Returns:
            Dictionary containing recommendations
        """
        prompt = f"""
        Aşağıdaki olay bağlamında hukuki öneriler ve eylem planı oluştur:
        
        Bağlam: {event_context}
        """
        
        if analysis_data and analysis_data.get("success"):
            prompt += f"\n\nÖnceki Analiz: {analysis_data.get('analysis', '')}"
        
        prompt += """
        
        Lütfen şunları sun:
        1. Acil yapılması gerekenler
        2. Toplanması gereken deliller
        3. Başvurulabilecek yasal yollar
        4. Dikkat edilmesi gereken hususlar
        """
        
        try:
            response = self.client.completions.create(
                model=self.model,
                prompt=prompt,
                max_tokens=1000,
                temperature=0.7,
                top_p=1.0
            )
            
            recommendations_text = response.choices[0].text.strip()
            
            return {
                "success": True,
                "recommendations": recommendations_text,
                "model": self.model,
                "tokens_used": response.usage.total_tokens
            }
        except Exception as e:
            return {
                "success": False,
                "error": str(e)
            }
    
    def extract_keywords(self, text: str) -> List[str]:
        """
        Extract legal keywords from text for case law search
        
        Args:
            text: Text to extract keywords from
            
        Returns:
            List of extracted keywords
        """
        prompt = f"""
        Aşağıdaki metinden Yargıtay karar araması için anahtar kelimeleri çıkar.
        Sadece anahtar kelimeleri virgülle ayırarak listele:
        
        Metin: {text}
        """
        
        try:
            response = self.client.completions.create(
                model=self.model,
                prompt=prompt,
                max_tokens=200,
                temperature=0.5
            )
            
            keywords_text = response.choices[0].text.strip()
            keywords = [k.strip() for k in keywords_text.split(",") if k.strip()]
            
            return keywords
        except Exception as e:
            print(f"Keyword extraction error: {e}")
            return []
