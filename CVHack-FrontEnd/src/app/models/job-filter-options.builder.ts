import { Job } from './job.model';
import { JobFilterOptions } from './job-filter-options.model';

export function buildJobFilterOptions(jobs: Job[]): JobFilterOptions {
    const citiesByCountry: Record<string, string[]> = {};
    for (const job of jobs) {
        if (!job.country) continue;
        (citiesByCountry[job.country] ??= []);
        if (job.city && !citiesByCountry[job.country].includes(job.city)) {
            citiesByCountry[job.country].push(job.city);
        }
    }
    for (const c of Object.keys(citiesByCountry)) citiesByCountry[c].sort();

    const mins = jobs.map((j) => j.salaryMin).filter(Number.isFinite);
    const maxs = jobs.map((j) => j.salaryMax).filter(Number.isFinite);

    return {
        countries: unique(jobs.map((j) => j.country)),
        citiesByCountry,
        workTypes: unique(jobs.map((j) => j.workType)),
        workTimes: unique(jobs.map((j) => j.workTime)),
        seniorities: unique(jobs.map((j) => j.seniority)),
        salaryMin: mins.length ? Math.floor(Math.min(...mins)) : 0,
        salaryMax: maxs.length ? Math.ceil(Math.max(...maxs)) : 200,
    };
}

function unique(values: string[]): string[] {
    return [...new Set(values.filter(Boolean))].sort();
}