export type OrientaPlaylistKey = 'profiles' | 'success-stories' | 'interviews';

export interface YoutubePlaylistSection {
  key: OrientaPlaylistKey;
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
