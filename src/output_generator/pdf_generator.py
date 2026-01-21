"""
PDF Report Generator
Generates PDF reports from analysis results using ReportLab
"""

from typing import Dict
from datetime import datetime
from reportlab.lib.pagesizes import A4
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.lib.units import cm
from reportlab.platypus import SimpleDocTemplate, Paragraph, Spacer, PageBreak
from reportlab.lib.enums import TA_CENTER, TA_LEFT, TA_JUSTIFY
from reportlab.pdfbase import pdfmetrics
from reportlab.pdfbase.ttfonts import TTFont


class PDFReportGenerator:
    """Generator for PDF formatted analysis reports"""
    
    def __init__(self):
        """Initialize PDF generator with styles"""
        self.styles = getSampleStyleSheet()
        self._setup_custom_styles()
    
    def _setup_custom_styles(self):
        """Setup custom paragraph styles"""
        # Title style
        self.styles.add(ParagraphStyle(
            name='CustomTitle',
            parent=self.styles['Heading1'],
            fontSize=18,
            textColor='#2C3E50',
            spaceAfter=30,
            alignment=TA_CENTER
        ))
        
        # Section header style
        self.styles.add(ParagraphStyle(
            name='SectionHeader',
            parent=self.styles['Heading2'],
            fontSize=14,
            textColor='#34495E',
            spaceAfter=12,
            spaceBefore=12
        ))
        
        # Body text style
        self.styles.add(ParagraphStyle(
            name='BodyText',
            parent=self.styles['Normal'],
            fontSize=10,
            alignment=TA_JUSTIFY,
            spaceAfter=12
        ))
    
    def generate_report(self, analysis_data: Dict, output_path: str) -> bool:
        """
        Generate PDF report from analysis data
        
        Args:
            analysis_data: Analysis results dictionary
            output_path: Path to save the PDF report
            
        Returns:
            True if successful, False otherwise
        """
        try:
            doc = SimpleDocTemplate(
                output_path,
                pagesize=A4,
                rightMargin=2*cm,
                leftMargin=2*cm,
                topMargin=2*cm,
                bottomMargin=2*cm
            )
            
            story = []
            
            # Title
            story.append(Paragraph(
                "Hırsızlık Suçları Analiz Raporu",
                self.styles['CustomTitle']
            ))
            story.append(Spacer(1, 0.5*cm))
            
            # Metadata
            story.append(Paragraph(
                f"Rapor Tarihi: {datetime.now().strftime('%d.%m.%Y %H:%M')}",
                self.styles['BodyText']
            ))
            story.append(Spacer(1, 0.3*cm))
            
            # Event Information
            story.append(Paragraph("Olay Bilgileri", self.styles['SectionHeader']))
            story.append(Paragraph(
                f"<b>Olay Türü:</b> {analysis_data.get('event_type', 'Belirtilmemiş')}",
                self.styles['BodyText']
            ))
            story.append(Paragraph(
                f"<b>Olay Açıklaması:</b> {self._escape_html(analysis_data.get('event_description', ''))}",
                self.styles['BodyText']
            ))
            story.append(Spacer(1, 0.5*cm))
            
            # OpenAI Analysis
            openai_data = analysis_data.get('openai_analysis', {})
            if openai_data.get('success'):
                story.append(Paragraph("OpenAI Analizi", self.styles['SectionHeader']))
                story.append(Paragraph(
                    self._escape_html(openai_data.get('analysis', '')),
                    self.styles['BodyText']
                ))
                story.append(Spacer(1, 0.5*cm))
            
            # Recommendations
            recommendations = analysis_data.get('recommendations', {})
            if recommendations.get('success'):
                story.append(Paragraph("Hukuki Öneriler", self.styles['SectionHeader']))
                story.append(Paragraph(
                    self._escape_html(recommendations.get('recommendations', '')),
                    self.styles['BodyText']
                ))
                story.append(Spacer(1, 0.5*cm))
            
            # Yargıtay Decisions
            yargitay = analysis_data.get('yargitay_decisions', {})
            if yargitay.get('success'):
                story.append(PageBreak())
                story.append(Paragraph("Yargıtay Kararları", self.styles['SectionHeader']))
                story.append(Paragraph(
                    self._escape_html(yargitay.get('decisions', '')),
                    self.styles['BodyText']
                ))
                story.append(Spacer(1, 0.5*cm))
            
            # Correlation Analysis
            correlation = analysis_data.get('correlation_analysis', {})
            if correlation.get('success'):
                story.append(Paragraph("İlişkilendirme Analizi", self.styles['SectionHeader']))
                story.append(Paragraph(
                    self._escape_html(correlation.get('correlation_analysis', '')),
                    self.styles['BodyText']
                ))
                story.append(Spacer(1, 0.5*cm))
            
            # Academic Sources
            academic = analysis_data.get('academic_sources', {})
            if academic.get('success'):
                story.append(PageBreak())
                story.append(Paragraph("Akademik Kaynaklar", self.styles['SectionHeader']))
                story.append(Paragraph(
                    self._escape_html(academic.get('academic_sources', '')),
                    self.styles['BodyText']
                ))
            
            # Build PDF
            doc.build(story)
            return True
            
        except Exception as e:
            print(f"PDF generation error: {e}")
            return False
    
    def _escape_html(self, text: str) -> str:
        """Escape HTML special characters"""
        if not text:
            return ""
        text = text.replace('&', '&amp;')
        text = text.replace('<', '&lt;')
        text = text.replace('>', '&gt;')
        return text
