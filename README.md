# Hırsızlık Suçları Analizi
## AI-Powered Legal Analysis System for Turkish Criminal Law

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

**Nitelikli Hırsızlık Suçlarının Anatomisi** - A comprehensive system that integrates OpenAI and DeepSeek AI to analyze theft crimes under Turkish Criminal Law, providing legal analysis, Supreme Court (Yargıtay) decisions, and academic sources.

## 🎯 Features

- **OpenAI Integration**: Advanced event analysis and legal recommendations using GPT models
- **DeepSeek AI Integration**: Supreme Court decision search and academic source correlation
- **Multi-format Output**: Generate reports in JSON, PDF, and Word (OpenXML) formats
- **Natural Language Processing**: Automatic keyword extraction and semantic analysis
- **Case Law Correlation**: Direct correlation between events and relevant Yargıtay decisions
- **Academic Research**: Automated search for relevant legal articles and scholarly sources

## 🏗️ Architecture

```
src/
├── ai_integration/          # AI service integrations
│   ├── openai_service.py   # OpenAI API wrapper
│   └── deepseek_service.py # DeepSeek AI wrapper
├── analysis/                # Analysis services
│   └── crime_analysis.py   # Main analysis orchestrator
├── output_generator/        # Report generators
│   ├── json_generator.py   # JSON reports
│   ├── pdf_generator.py    # PDF reports
│   └── openxml_generator.py # Word documents
└── utils/                   # Utilities
    └── config.py           # Configuration management
```

## 📋 Requirements

- Python 3.8+
- OpenAI API key
- DeepSeek AI API key

## 🚀 Installation

1. Clone the repository:
```bash
git clone https://github.com/acaroktay1967-crypto/H-rs-zl-k_Suclar-_Analizi.git
cd H-rs-zl-k_Suclar-_Analizi
```

2. Install dependencies:
```bash
pip install -r requirements.txt
```

3. Configure API keys:
```bash
cp config/.env.example .env
# Edit .env and add your API keys
```

## 🔧 Configuration

Create a `.env` file with your API keys:

```env
OPENAI_API_KEY=your_openai_api_key_here
OPENAI_MODEL=text-davinci-003

DEEPSEEK_API_KEY=your_deepseek_api_key_here
DEEPSEEK_API_BASE=https://api.deepseek.com/v1

OUTPUT_FORMAT=json
OUTPUT_DIR=./reports
```

## 💻 Usage

### Command Line Interface

```bash
# Basic analysis
python main.py "Olay açıklaması buraya yazılır"

# Specify crime type
python main.py "Olay açıklaması" -t "nitelikli hırsızlık"

# Generate PDF report
python main.py "Olay açıklaması" -f pdf -o rapor.pdf

# Generate Word document
python main.py "Olay açıklaması" -f docx -o rapor.docx

# Quick analysis (OpenAI only)
python main.py "Olay açıklaması" -q
```

### Python API

```python
from src.analysis.crime_analysis import CrimeAnalysisService
from src.output_generator.json_generator import JSONReportGenerator

# Initialize service
service = CrimeAnalysisService()

# Analyze event
event = "Gece vakti konuta girerek hırsızlık yapan şüpheli yakalanmıştır."
result = service.analyze_crime_event(event, "nitelikli hırsızlık")

# Generate report
generator = JSONReportGenerator()
json_report = generator.generate_report(result, "rapor.json")
```

## 📚 Examples

See `examples/usage_examples.py` for detailed examples:

```bash
python examples/usage_examples.py
```

## 🔍 API Reference

### CrimeAnalysisService

Main service for crime analysis:

```python
service = CrimeAnalysisService(openai_key=None, deepseek_key=None)
```

**Methods:**
- `analyze_crime_event(event_description, event_type)`: Full analysis with both AI services
- `get_quick_analysis(event_description)`: Quick analysis using OpenAI only
- `search_case_law(keywords, event_type)`: Search Yargıtay decisions

### OpenAIService

OpenAI integration service:

```python
service = OpenAIService(api_key=None, model="text-davinci-003")
```

**Methods:**
- `analyze_event(event_description, event_type)`: Analyze criminal event
- `generate_recommendations(event_context, analysis_data)`: Generate legal recommendations
- `extract_keywords(text)`: Extract keywords for case law search

### DeepSeekService

DeepSeek AI integration service:

```python
service = DeepSeekService(api_key=None, api_base=None)
```

**Methods:**
- `search_yargitay_decisions(keywords, event_type)`: Search Supreme Court decisions
- `search_academic_sources(event_context, keywords)`: Search academic sources
- `correlate_decisions(event_description, decisions_data)`: Correlate decisions with event

## 📊 Output Formats

### JSON
Structured data format suitable for integration with other systems.

### PDF
Professional report format with formatted sections and styling.

### Word (DOCX)
Editable document format using OpenXML standards.

## 🛡️ Security

- API keys are managed through environment variables
- No sensitive data is stored in the repository
- All API communications use HTTPS

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Author

**besiktaş1903**

## 🙏 Acknowledgments

- OpenAI for providing powerful language models
- DeepSeek AI for legal analysis capabilities
- Turkish legal system and Yargıtay for case law resources

## 📞 Support

For issues and questions, please open an issue on GitHub.

---

**Note**: This system is designed to assist legal analysis but should not replace professional legal advice. Always consult with a qualified attorney for legal matters.
