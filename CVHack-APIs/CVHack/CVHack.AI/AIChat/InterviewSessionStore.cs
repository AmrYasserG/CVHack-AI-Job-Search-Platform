using System.Collections.Concurrent;

namespace CVHack.AI.AIChat;

// تخزين بسيط للجلسات الجارية في الذاكرة (من غير داتابيز)
public class InterviewSessionStore
{
    private readonly ConcurrentDictionary<Guid, InterviewSession> _sessions = new();

    public void Save(InterviewSession session) => _sessions[session.Id] = session;

    public InterviewSession? Get(Guid id) =>
        _sessions.TryGetValue(id, out var session) ? session : null;

    public void Remove(Guid id) => _sessions.TryRemove(id, out _);
}