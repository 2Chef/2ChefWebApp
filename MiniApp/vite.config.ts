import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],

    build: {
        outDir: '../WebApp/wwwroot/miniapp',
    },

    server: {
        port: 443,
    }
});
