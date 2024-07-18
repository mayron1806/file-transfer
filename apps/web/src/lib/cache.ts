import { LRUCache } from 'lru-cache';
const cache = new LRUCache({
  ttl: 1000 * 60, // 1 minute
  max: 1000,
  updateAgeOnGet: false,
  updateAgeOnHas: false
});
export default {
  get: (key: string) => {
    const data = cache.get(key);
    if (data) console.log('[CACHE] - HIT:', key);
    else console.log('[CACHE] - MISS:', key);
    return data;
  },
  set: (key: string, value: any) => {
    console.log('[CACHE] - SET:', key);
    cache.set(key, value)
  },
  delete: (key: string) => {
    const deleted = cache.delete(key);
    console.log('[CACHE] - DELETE:', key);
    return deleted;
  },
  clear: () => {
    console.log('[CACHE] - CLEARED');
    cache.clear();
  },
};