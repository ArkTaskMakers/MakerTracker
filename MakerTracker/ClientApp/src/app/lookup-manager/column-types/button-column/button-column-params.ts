export interface ButtonColumnParams {
  type: 'action' | 'link';
  action?: (params: ButtonColumnParams) => void;
  route?: (params: ButtonColumnParams) => any[];
  color?: string;
  tooltip: string;
  icon: string;
  value?: any;
  data?: any;
}
