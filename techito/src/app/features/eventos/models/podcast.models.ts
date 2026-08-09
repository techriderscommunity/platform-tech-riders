export interface PodcastVideoDto {
  videoId: string;
  title: string;
  url: string;
  embedUrl: string;
  publishedAt?: string | null;
  thumbnailUrl?: string | null;
}
