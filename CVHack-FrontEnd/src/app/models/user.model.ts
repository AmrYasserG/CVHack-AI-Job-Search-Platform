export interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  roles: string[];
  plan: string;     // 'Free' | 'Pro'
  status: string;   // 'Active' | 'Suspended'
  searches: number;
  createdAt: string;
}