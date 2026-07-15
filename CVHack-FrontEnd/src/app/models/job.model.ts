export interface Job {
  id: number;
  initials: string;
  title: string;
  company: string;
  city: string;
  country: string;
  location: string;
  postedAgo: string;
  postedAt: string;
  workType: string;
  workTime: string;
  seniority: string;
  tags: string[];
  salaryMin: number;
  salaryMax: number;
  matchScore: number;
  description: string;
  briefDescription: string;
  jobUrl: string;
  sourcePlatform: string;
  responsibilities: string[];
  requirements: string[];
}