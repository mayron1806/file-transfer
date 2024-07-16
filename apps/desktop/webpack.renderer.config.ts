// @ts-nocheck
import type { Configuration } from 'webpack';

import { rules } from './webpack.rules';
import { plugins } from './webpack.plugins';
import path from 'path';

export const rendererConfig: Configuration = {
  module: {
    rules,
  },
  plugins,
  resolve: {
    extensions: ['.js', '.ts', '.jsx', '.tsx', '.css'],
    alias: {
      '@/components': path.resolve(__dirname, 'src/renderer/components'),
      '@/lib': path.resolve(__dirname, 'src/renderer/lib'),
      '@/hooks': path.resolve(__dirname, 'src/renderer/hooks'),
      '@/assets': path.resolve(__dirname, 'assets'),
      '@/validation': path.resolve(__dirname, 'src/renderer/validation'),
    }
  },
};
