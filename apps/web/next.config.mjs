const serverURL = process.env.API_URL;
/** @type {import('next').NextConfig} */
const nextConfig = {
  webpack: (config) => {
    config.module.rules.push({
      test: /\.svg$/,
      use: ["@svgr/webpack"]
    });

    return config;
  },
  rewrites: async () => {
    return [
      {
        // Isso redireciona todas as requisições de /api para a API externa, exceto /api/auth
        source: '/api/:path((?!auth).*)',
        destination: `${serverURL}/:path*`,
      },
      {
        // Isso deixa /api/auth intacto
        source: '/api/auth/:path*',
        destination: '/api/auth/:path*',
      },
    ]
  },
  reactStrictMode: false,
};

export default nextConfig;
