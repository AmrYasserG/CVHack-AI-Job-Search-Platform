import { Component } from '@angular/core';
import { AiStarsIcon } from "../../assets/ai-stars-icon/ai-stars-icon";
import { CompanyIcon } from '../../assets/company-icon/company-icon';
import { MicIcon } from '../../assets/mic-icon/mic-icon';
import { GraphIcon } from '../../assets/graph-icon/graph-icon';

@Component({
  selector: 'app-feature-pills',
  imports: [AiStarsIcon, GraphIcon, MicIcon, CompanyIcon],
  templateUrl: './feature-pills.html',
  styleUrl: './feature-pills.css',
})
export class FeaturePills {
  
}
