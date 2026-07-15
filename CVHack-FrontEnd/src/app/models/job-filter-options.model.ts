export interface JobFilterOptions {
  countries: string[];
  citiesByCountry: Record<string, string[]>;
  workTypes: string[];
  workTimes: string[];
  seniorities: string[];
  salaryMin: number;  
  salaryMax: number;   
}