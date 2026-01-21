"""
DeepSeek AI Integration Service
Provides Supreme Court decision search and academic source correlation
"""

import os
import requests
from typing import Dict, List, Optional


class DeepSeekService:
    """Service for interacting with DeepSeek AI API"""
    
    def __init__(self, api_key: Optional[str] = None, api_base: Optional[str] = None):
        """
        Initialize DeepSeek service
        
        Args:
            api_key: DeepSeek API key. If None, reads from DEEPSEEK_API_KEY env variable
            api_base: DeepSeek API base URL. If None, reads from DEEPSEEK_API_BASE env variable
        """
        self.api_key = api_key or os.getenv("DEEPSEEK_API_KEY")
        if not self.api_key:
            raise ValueError("DeepSeek API key must be provided or set in DEEPSEEK_API_KEY environment variable")
        
        self.api_base = api_base or os.getenv("DEEPSEEK_API_BASE", "https://api.deepseek.com/v1")
        self.headers = {
            "Authorization": f"Bearer {self.api_key}",
            "Content-Type": "application/json"
        }
    
    def search_yargitay_decisions(self, keywords: List[str], event_type: str) -> Dict:
        """
        Search for relevant Yargıtay (Supreme Court) decisions
        
        Args:
            keywords: List of keywords to search for
            event_type: Type of criminal event
            
        Returns:
            Dictionary containing search results
        """
        # Create search query combining keywords and event type
        search_query = f"""
        Türk Yargıtayı'ndan {event_type} suçu ile ilgili şu anahtar kelimeleri içeren 
        kararları ara ve özetle:
        
        Anahtar Kelimeler: {', '.join(keywords)}
        
        Lütfen şunları içeren kararları bul:
        1. İlgili Yargıtay dairesi ve karar numarası
        2. Kararın özeti
        3. Hukuki değerlendirme
        4. İçtihat notu
        """
        
        try:
            response = requests.post(
                f"{self.api_base}/chat/completions",
                headers=self.headers,
                json={
                    "model": "deepseek-chat",
                    "messages": [
                        {
                            "role": "system",
                            "content": "Sen Türk hukuku ve Yargıtay içtihatları konusunda uzman bir hukuk asistanısın."
                        },
                        {
                            "role": "user",
                            "content": search_query
                        }
                    ],
                    "max_tokens": 2000,
                    "temperature": 0.7
                },
                timeout=30
            )
            
            if response.status_code == 200:
                result = response.json()
                decisions_text = result.get("choices", [{}])[0].get("message", {}).get("content", "")
                
                return {
                    "success": True,
                    "keywords": keywords,
                    "event_type": event_type,
                    "decisions": decisions_text,
                    "source": "deepseek"
                }
            else:
                return {
                    "success": False,
                    "error": f"API Error: {response.status_code} - {response.text}",
                    "keywords": keywords
                }
        except Exception as e:
            return {
                "success": False,
                "error": str(e),
                "keywords": keywords
            }
    
    def search_academic_sources(self, event_context: str, keywords: List[str]) -> Dict:
        """
        Search for relevant academic sources and legal articles
        
        Args:
            event_context: Context of the event
            keywords: List of keywords for search
            
        Returns:
            Dictionary containing academic source results
        """
        search_query = f"""
        Aşağıdaki olay bağlamında ilgili akademik makaleleri, hukuk dergilerini ve 
        doktriner görüşleri ara:
        
        Olay Bağlamı: {event_context}
        Anahtar Kelimeler: {', '.join(keywords)}
        
        Lütfen şunları içeren kaynakları bul:
        1. Yazar ve makale adı
        2. Yayın bilgileri
        3. İlgili bölüm özeti
        4. Olayla ilişkisi
        """
        
        try:
            response = requests.post(
                f"{self.api_base}/chat/completions",
                headers=self.headers,
                json={
                    "model": "deepseek-chat",
                    "messages": [
                        {
                            "role": "system",
                            "content": "Sen Türk ceza hukuku akademik literatürü konusunda uzman bir araştırmacısın."
                        },
                        {
                            "role": "user",
                            "content": search_query
                        }
                    ],
                    "max_tokens": 2000,
                    "temperature": 0.7
                },
                timeout=30
            )
            
            if response.status_code == 200:
                result = response.json()
                sources_text = result.get("choices", [{}])[0].get("message", {}).get("content", "")
                
                return {
                    "success": True,
                    "event_context": event_context,
                    "keywords": keywords,
                    "academic_sources": sources_text,
                    "source": "deepseek"
                }
            else:
                return {
                    "success": False,
                    "error": f"API Error: {response.status_code} - {response.text}"
                }
        except Exception as e:
            return {
                "success": False,
                "error": str(e)
            }
    
    def correlate_decisions(self, event_description: str, decisions_data: str) -> Dict:
        """
        Correlate Yargıtay decisions with the specific event
        
        Args:
            event_description: Description of the event
            decisions_data: Raw decisions data from previous search
            
        Returns:
            Dictionary containing correlation analysis
        """
        correlation_query = f"""
        Aşağıdaki olay ile Yargıtay kararları arasında doğrudan ilişki kur:
        
        Olay: {event_description}
        
        Yargıtay Kararları: {decisions_data}
        
        Lütfen şunları analiz et:
        1. Hangi kararlar bu olayla doğrudan ilgili
        2. Benzerlikler ve farklılıklar
        3. Kararların olay için anlamı
        4. Olası hukuki sonuçlar
        """
        
        try:
            response = requests.post(
                f"{self.api_base}/chat/completions",
                headers=self.headers,
                json={
                    "model": "deepseek-chat",
                    "messages": [
                        {
                            "role": "system",
                            "content": "Sen Yargıtay içtihatlarını somut olaylarla ilişkilendiren uzman bir hukukçusun."
                        },
                        {
                            "role": "user",
                            "content": correlation_query
                        }
                    ],
                    "max_tokens": 1500,
                    "temperature": 0.7
                },
                timeout=30
            )
            
            if response.status_code == 200:
                result = response.json()
                correlation_text = result.get("choices", [{}])[0].get("message", {}).get("content", "")
                
                return {
                    "success": True,
                    "event_description": event_description,
                    "correlation_analysis": correlation_text,
                    "source": "deepseek"
                }
            else:
                return {
                    "success": False,
                    "error": f"API Error: {response.status_code} - {response.text}"
                }
        except Exception as e:
            return {
                "success": False,
                "error": str(e)
            }
