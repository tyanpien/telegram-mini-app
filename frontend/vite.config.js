import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    allowedHosts: ['bf64d7981befb0e8c049b83b93872c1c.serveo.net'],
    host: true,
    port: 3000
  }
})
