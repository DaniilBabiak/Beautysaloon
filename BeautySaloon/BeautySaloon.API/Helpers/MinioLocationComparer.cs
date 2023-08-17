using BeautySaloon.API.RabbitMq.Models;

namespace BeautySaloon.API.Helpers;

public class MinioLocationComparer : IEqualityComparer<MinioLocation>
{
    public bool Equals(MinioLocation x, MinioLocation y)
    {
        if (x == null || y == null)
            return false;

        return x.BucketName == y.BucketName && x.FileName == y.FileName;
    }

    public int GetHashCode(MinioLocation obj)
    {
        return (obj.BucketName + obj.FileName).GetHashCode();
    }
}
