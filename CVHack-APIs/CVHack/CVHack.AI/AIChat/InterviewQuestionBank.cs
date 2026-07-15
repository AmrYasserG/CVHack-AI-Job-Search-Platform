using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

// بوابة خاصة بالـ Interview Coach للوصول لمكتبة الأسئلة في RAG
// بتستخدم الـ interfaces الموجودة زي ما هي، وتفلتر بالـ Category بنفسها

namespace CVHack.AI.AIChat
{
    public class InterviewQuestionBank
    {
        private readonly IEmbeddingService _embedder;
        private readonly IVectorStore _store;
        private const string KnowledgeBase = "InterviewQuestions";

        public InterviewQuestionBank(IEmbeddingService embedder, IVectorStore store)
        {
            _embedder = embedder;
            _store = store;
        }

        // بيرجع أقرب النصوص (chunks) المتعلقة بمستوى وتراك الوظيفة، كنص واحد جاهز يتحط في البرومبت
        public async Task<string> GetContextAsync(string jobTitle, string seniority, string focusQuery, int topK = 6, CancellationToken ct = default)
        {
            var category = InterviewTrackMapper.ResolveCategory(jobTitle);
            var level = InterviewTrackMapper.ResolveLevel(seniority);

            // بنضيف التراك والمستوى للاستعلام نفسه عشان الـ embedding يبقى أدق
            var query = $"{category} {level} level interview questions: {focusQuery}";
            var queryVector = await _embedder.EmbedAsync(query, ct);

            // بنسحب عدد أكبر من اللي محتاجينه، عشان نقدر نفلتر بالـ Category و الملف بعدين من غير ما نلمس الـ store
            var raw = await _store.SearchAsync(queryVector, KnowledgeBase, topK: topK * 4, ct: ct);

            var filtered = raw
                .Where(r => r.Chunk.Category == category)
                .Where(r => r.Chunk.SourceFile.StartsWith(level, StringComparison.OrdinalIgnoreCase))
                .Take(topK)
                .ToList();

            // لو الفلترة الدقيقة رجعت فاضية (مثلاً تراك نادر)، نرجع لأقرب نتايج عامة بدل ما نرجع فاضي
            if (filtered.Count == 0)
                filtered = raw.Take(topK).ToList();

            return string.Join("\n\n", filtered.Select((r, i) =>
                $"[{i + 1}] [{r.Chunk.Category}/{r.Chunk.SourceFile}]\n{StripQuestionNumber(r.Chunk.Text)}"));
        }

        private static string StripQuestionNumber(string text) =>
            Regex.Replace(text.Trim(), @"^Q?\d+[\.\):]\s*", "", RegexOptions.IgnoreCase);
    }
}