import { JobDto } from './job.dto.model';
import { Job } from './job.model';

export function mapJobDtoToJob(dto: JobDto): Job {
  return {
    id: dto.id,
    initials: companyInitials(dto.companyName),
    title: dto.title,
    company: dto.companyName,
    city: dto.city,
    country: dto.country,
    location: [dto.city, dto.country].filter(Boolean).join(', '),
    postedAgo: timeAgo(dto.postedAt),
    postedAt: dto.postedAt,
    workType: dto.workType,
    workTime: dto.workTime,
    seniority: dto.seniority,
    tags: [dto.workType, dto.workTime, dto.seniority].filter(Boolean),
    salaryMin: dto.salaryMin,
    salaryMax: dto.salaryMax,
    matchScore: 0,
    description: dto.description,
    briefDescription: dto.briefDescription ?? '',
    jobUrl: dto.jobUrl ?? '',         // ✅ زيادة
    sourcePlatform: dto.sourcePlatform ?? '', // ✅ زيادة
    responsibilities: dto.responsibilities ?? [],
    requirements: dto.requirements ?? [],
  };
}

function companyInitials(company: string): string {
  const parts = company.trim().split(/\s+/);
  const letters = parts.length >= 2 ? parts[0][0] + parts[1][0] : company.slice(0, 2);
  return letters.toUpperCase();
}

function timeAgo(dateStr: string): string {
  const hours = Math.floor((Date.now() - new Date(dateStr).getTime()) / 36e5);
  if (hours < 1) return 'just now';
  if (hours < 24) return `${hours}h ago`;
  const days = Math.floor(hours / 24);
  if (days < 7) return `${days}d ago`;
  return `${Math.floor(days / 7)}w ago`;
}