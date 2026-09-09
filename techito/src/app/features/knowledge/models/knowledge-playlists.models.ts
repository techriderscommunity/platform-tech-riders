export type KnowledgePlaylistKey = 'profiles' | 'success-stories' | 'interviews';

export interface YoutubePlaylistSection {
  key: KnowledgePlaylistKey;
  title: string;
  url: string;
}

export interface PlaylistVideoDto {
  videoId: string;
  title: string;
  url: string;
  embedUrl: string;
  publishedAt?: string | null;
  thumbnailUrl?: string | null;
}
