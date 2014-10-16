using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using ISeeYou.Documents;
using ISeeYou.Domain.Aggregates.Subject.Commands;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace ISeeYou.Ranking
{
    public class PhotoRanker2
    {
        private readonly SourcesViewService _sources;
        private readonly FetchingStatsService _stats;
        private readonly SourceStatsViewService _sourceStats;
        private readonly PhotoDocumentsService _photoDocuments;

        public PhotoRanker2(SourcesViewService sources, FetchingStatsService stats, SourceStatsViewService sourceStats, PhotoDocumentsService photoDocuments)
        {
            _sources = sources;
            _stats = stats;
            _sourceStats = sourceStats;
            _photoDocuments = photoDocuments;
        }

        public void Run()
        {
            var now = DateTime.UtcNow;
            var minDelay = TimeSpan.FromSeconds(300);

            while (true)
            {
                var sourceDocument = _sourceStats.Items.FindAndModify(Query<SourceStats>.LT(x => x.Fetched, now.Add(minDelay)),
                    SortBy<SourceStats>.Ascending(x => x.Fetched),
                    Update<SourceStats>.Inc(x => x.Count, 1).Set(x => x.Fetched, now))
                    .GetModifiedDocumentAs<SourceStats>();
                if (sourceDocument != null)
                {
                    var stopWatch = new Stopwatch();
                    stopWatch.Start();
                    List<int> subjectIds = null;
                    try
                    {
                        var sourceId = sourceDocument.SourceId;
                        subjectIds = GetSubjects(sourceDocument.SourceId);
                        var api = new VkApi();
                        var albums =
                            api.GetAlbums(sourceId)
                                .Select(x => x.aid.ToString(CultureInfo.InvariantCulture))
                                .Concat(new[] { "profile", "wall" });
                        foreach (var album in albums)
                        {
                            try
                            {
                                var photos =
                                    api.GetPhotos(sourceId, album)
                                        .Where(x => x.likes.count > 0)
                                        .OrderByDescending(x => x.created);
                                foreach (var photoDto in photos)
                                {
                                    try
                                    {
                                        const string type = "photo";
                                        var id = sourceId + "_" + type + photoDto.pid;
                                        //_photoDocuments.Items.Update(Query<PhotoDocument>.EQ(x=> x.Id, id), Update<PhotoDocument>.Set(x=> x.NextFetching)),
                                        var created = UnixTimeStampToDateTime(photoDto.created);
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //_sourceStats.Items.Update(Query<SourceStats>.EQ(x => x.SourceId, sourceDocument.SourceId),
                        //    Update<SourceStats>.Inc(x => x.Count, -1).Set(x => x.Fetched, sourceDocument.Fetched));

                    }
                    finally
                    {
                        stopWatch.Stop();
                        //Save also arrours occured during fetching and resource id
                        SaveStats(sourceDocument, subjectIds, stopWatch.ElapsedMilliseconds);
                    }
                }
            }
        }

        private void SaveStats(SourceStats sourceDocument, List<int> subjectIds, long elapsedMilliseconds)
        {
            _stats.Items.Insert(new FetchingStats
            {
                Id = ObjectId.GenerateNewId().ToString(),
                SourceId = sourceDocument.SourceId,
                Subjects = subjectIds,
                //  Rank = sourceDocument.Rank,
                Processed = DateTime.UtcNow,
                Elapsed = elapsedMilliseconds
            });
        }

        private List<int> GetSubjects(int id)
        {
            return _sources.Items.Find(Query<SourceDocument>.EQ(x => x.SourceId, id)).SetFields("SubjectId").Select(x => x.SubjectId).ToList();
        }


        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        } 
    }

}