export interface JobDto {
  id: number;
  title: string;
  companyName: string;
  city: string;            
  country: string;
  seniority: string;       
  workType: string;
  workTime: string;
  description: string;
  briefDescription: string | null;
  salaryMin: number;
  salaryMax: number;
  postedAt: string;
  jobUrl: string;
  sourcePlatform: string;
  responsibilities: string[];
  requirements: string[];
}