using System;
using System.Collections;
using UnityEngine;

public static class Util
{
    public static float CalculateSpeedFromHeight(float height, float gravity)
    {
        return Mathf.Sqrt(height * 2f * gravity);
    }

    /// <summary>
    /// 检查对象是否面朝左边
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static bool CheckIsFaceLeft(this Transform transform, Vector3 frontAxis = default(Vector3))
    {
        Vector3 front;
        if (frontAxis != default(Vector3))
        {
            front = frontAxis;
        }
        else
        {
            front = transform.forward;
        }
        var angle = Util.AngleSigned(Vector3.forward, front, Vector3.up);
        return (angle < 0) ? true : false;
    }

    /// <summary>
    /// 检查目标点是否在对象前面
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static bool CheckIsFront(this Transform transform, Vector3 target)
    {
        Vector3 dir = target - transform.position;
        var result = Vector3.Dot(transform.forward, dir);
        return (result >= 0) ? true : false;
    }

    /// <summary>
    /// 播放Animation动画（指定是否受timescale影响）
    /// </summary>
    public static IEnumerator Play(Animation animation, string clipName, bool useTimeScale, Action onComplete)
    {
        if (!useTimeScale)
        {
            AnimationState _currState = animation[clipName];
            bool isPlaying = true;
            float _startTime = 0F;
            float _progressTime = 0F;
            float _timeAtLastFrame = 0F;
            float _timeAtCurrentFrame = 0F;
            float deltaTime = 0F;
            animation.Play(clipName);
            _timeAtLastFrame = Time.realtimeSinceStartup;
            while (isPlaying)
            {
                _timeAtCurrentFrame = Time.realtimeSinceStartup;
                deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
                _timeAtLastFrame = _timeAtCurrentFrame;

                _progressTime += deltaTime;
                _currState.normalizedTime = _progressTime / _currState.length;
                animation.Sample();
                if (_progressTime >= _currState.length)
                {
                    if (_currState.wrapMode != WrapMode.Loop)
                    {
                        isPlaying = false;
                    }
                    else
                    {
                        _progressTime = 0.0f;
                    }
                }
                yield return new WaitForEndOfFrame();
            }
            yield return null;
            if (onComplete != null)
            {
                onComplete();
            }
        }
        else
        {
            animation.Play(clipName);
        }
    }

    /// <summary>
    /// 获取一个动画片段的长度
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="clipName"></param>
    /// <returns></returns>
    public static float GetClipLength(this Animator animator, string clipName)
    {
        AnimationClip clip = null;
        if (null == animator || string.IsNullOrEmpty(clipName) || null == animator.runtimeAnimatorController)
            return float.MaxValue;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        AnimationClip[] tAnimationClips = ac.animationClips;
        if (null == tAnimationClips || tAnimationClips.Length <= 0)
        {
            return float.MaxValue;
        }
        for (int i = 0; i < tAnimationClips.Length; i++)
        {
            var tAnimationClip = ac.animationClips[i];
            if (null != tAnimationClip && tAnimationClip.name == clipName)
            {
                clip = tAnimationClip;
                break;
            }
        }
        return clip.length;
    }

    /// <summary>
    /// 获取一个动画片段的总帧数
    /// </summary>
    /// <param name="clip"></param>
    /// <returns></returns>
    public static float ClipFrames(this AnimationClip clip)
    {
        return clip.frameRate * clip.length;
    }

    /// <summary>
    /// 获取一个动画片段的总帧数
    /// </summary>
    /// <param name="animator">动画控制器</param>
    /// <param name="clipName">动画片段名</param>
    /// <returns></returns>
    public static float ClipFrames(this Animator animator, string clipName)
    {
        var time = 0f;
        var clip = animator.GetClip(clipName);
        if (clip == null)
        {
            return time;
        }
        return clip.frameRate * clip.length;
    }
    /// <summary>
    /// 获取动画片段
    /// </summary>
    /// <param name="animator">控制器</param>
    /// <param name="clipName">片段名</param>
    /// <returns></returns>
    public static AnimationClip GetClip(this Animator animator, string clipName)
    {
        AnimationClip clip = null;
        if (null == animator || string.IsNullOrEmpty(clipName) || null == animator.runtimeAnimatorController)
            return clip;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        AnimationClip[] tAnimationClips = ac.animationClips;
        if (null == tAnimationClips || tAnimationClips.Length <= 0)
        {
            return clip;
        }
        for (int i = 0; i < tAnimationClips.Length; i++)
        {
            var tAnimationClip = ac.animationClips[i];
            if (null != tAnimationClip && tAnimationClip.name == clipName)
            {
                clip = tAnimationClip;
                break;
            }
        }
        return clip;
    }

    /// <summary>
    /// 顺时针旋转取到两个向量之间的夹角角度, 水平向右方向为360度
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static float Angle360(Vector3 from, Vector3 to)
    {
        Vector3 v3 = Vector3.Cross(from, to);
        if (v3.z > 0)
        {
            return Vector3.Angle(from, to);
        }
        else
        {
            return 360 - Vector3.Angle(from, to);
        }

    }


    /// <summary>
    /// 返回目标是否在屏幕内
    /// </summary>
    /// <param name="trans">目标</param>
    /// <param name="X_OffsetLeft">X轴左偏移补正</param>
    /// <param name="X_OffsetRight">X轴右偏移补正</param>
    /// <param name="Y_OffsetUp">Y轴上偏移补正</param>
    /// <param name="Y_OffsetDown">Y轴下偏移补正</param>
    /// <returns></returns>
    public static bool IsInScreen(Transform trans, float X_OffsetLeft, float X_OffsetRight, float Y_OffsetUp, float Y_OffsetDown)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(trans.position);

        if ((screenPos.x + X_OffsetRight > Screen.width) || (screenPos.x + X_OffsetLeft < 0)) {
            return true;
        }
        else
        {
            if ((screenPos.y + Y_OffsetUp > Screen.height) || (screenPos.y + Y_OffsetDown < 0))
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// 返回目标点范围内是否存在指定层的碰撞器
    /// </summary>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static bool CalculateExistColliders(Vector3 target, float radius, LayerMask layerMask)
    {
        Collider[] colliders = UnityEngine.Physics.OverlapSphere(target, radius);
        int count = 0;
        foreach (Collider col in colliders)
        {
            if (((1 << col.gameObject.layer) & layerMask) > 0)
            {
                count++;
                break;
            }
        }

        if (count > 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 判断目标层是否在指定的LayerMask中
    /// </summary>
    /// <param name="layer">目标层</param>
    /// <param name="layerMask">指定的LayerMask</param>
    /// <returns></returns>
    public static bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        // 根据Layer数值进行移位获得用于运算的Mask值
        int objLayerMask = 1 << layer;
        return (layerMask.value & objLayerMask) > 0;
    }

    /// <summary>
    /// 计算两个向量的有符号夹角
    /// </summary>
    /// <param name="from">原向量</param>
    /// <param name="to">目标向量</param>
    /// <param name="axis">围绕的旋转轴</param>
    /// <returns></returns>
    public static float AngleSigned(Vector3 from, Vector3 to, Vector3 axis)
    {
        return Mathf.Atan2(
            Vector3.Dot(axis, Vector3.Cross(from, to)),
            Vector3.Dot(from, to)) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// 增加动画事件
    /// </summary>
    /// <param name="clipName">动画片段名</param>
    /// <param name="triggerIndex">触发帧数, -1为最后一帧</param>
    /// <param name="eventIndex">事件索引</param>
    public static void TryAddAnimationEvent(this Animator animator, string clipName, int triggerIndex, int eventIndex)
    {
        var clip = animator.GetClip(clipName);
        if (clip == null)
        {
            Debug.LogErrorFormat("错误, 没有该动画片段: {0}", clipName);
            return;
        }
        var totalFrame = clip.ClipFrames();
        var index = 0f;
        string evenName = string.Format("{0}{1}", "OnAnimationEvent", eventIndex);
        AnimationEvent animEvent = new AnimationEvent();
        animEvent.functionName = evenName;

        if (triggerIndex < 0)
        {
            index = totalFrame;
        }
        else
        {
            index = triggerIndex;
        }
        animEvent.time = (index / totalFrame) * clip.length;
        clip.AddEvent(animEvent);
    }

    /// <summary>
    /// 获取帧事件的关键帧时长
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="clipName"></param>
    /// <param name="frameEventList">帧事件列表</param>
    /// <param name="evenIndex">事件索引</param>
    /// <returns></returns>
    public static float TryGetFrameTime(this Animator animator, string clipName, int[] frameEventList, int evenIndex)
    {
        float time = 0f;
        if (animator == null || evenIndex >= frameEventList.Length)
        {
            return time;
        }
        var start = 0;
        var end = frameEventList[evenIndex];
        time = animator.TryGetFrameTime(clipName, start, end);
        return time;

    }

    /// <summary>
    /// 获取指定帧间隔的时间长度, 区间[]
    /// </summary>
    /// <param name="animator">动画控制器</param>
    /// <param name="clipName">动画片段名</param>
    /// <param name="frameStart">起始帧</param>
    /// <param name="frameEnd">结束帧</param>
    /// <returns></returns>
    public static float TryGetFrameTime(this Animator animator, string clipName, int frameStart, int frameEnd)
    {
        float time = 0f;
        var clip = animator.GetClip(clipName);
        if (clip == null)
        {
            Debug.LogErrorFormat("错误, 没有该动画片段: {0}", clipName);
            return time;
        }
        var totalFrame = clip.ClipFrames();
        if (totalFrame <= 0)
        {
            totalFrame = 1;
        }
        var end = frameEnd < 0 ? totalFrame : frameEnd;
        var delta = Mathf.Max(0, end - frameStart + 1);
        time = delta / clip.frameRate;
        return time;
    }

    /// <summary>
    /// 计算一条射线与平面的交点
    /// </summary>
    /// <param name="ray0">射线原点</param>
    /// <param name="rayD">射线方向</param>
    /// <param name="planeNormal">目标平面法向量</param>
    /// <param name="planePoint">目标平面一点</param>
    /// <returns></returns>
    public static Vector3 Calculateintersect(Vector3 ray0, Vector3 rayD, Vector3 planeNormal, Vector3 planePoint)
    {
        var p0 = ray0;
        var pd = rayD;
        var n = planeNormal;
        var r = Vector3.Dot(planePoint, n);
        float t = 0f;
        //射线方向与目标平面平行, 永远不可能相交
        if (Vector3.Dot(pd, n) == 0)
        {
            t = 0f;
        }
        else
        {
            t = (r - Vector3.Dot(p0, n)) / Vector3.Dot(pd, n);
        }
        Vector3 intersect = p0 + t * pd;
        return intersect;
    }

    /// <summary>
    /// 2D平面直角坐标系中的向量旋转
    /// </summary>
    /// <param name="v">向量</param>
    /// <param name="angle">旋转角(degree)(顺时针为正)</param>
    /// <returns></returns>
    public static Vector2 CalculateVectorRotate2D(Vector2 v, float angle)
    {
        var x1 = v.x * Mathf.Cos(angle * Mathf.Deg2Rad) + v.y * Mathf.Sin(angle * Mathf.Deg2Rad);
        var y1 = v.y * Mathf.Cos(angle * Mathf.Deg2Rad) - v.x * Mathf.Sin(angle * Mathf.Deg2Rad);

        return new Vector2(x1, y1);
    }

    /// <summary>
    /// 直线和圆的交点
    /// </summary>
    /// <param name="ptStart">线段起点</param>
    /// <param name="ptEnd">线段终点</param>
    /// <param name="ptCenter">圆心坐标</param>
    /// <param name="Radius2">圆半径平方</param>
    /// <param name="ptInter1">交点1(若不存在返回0)</param>
    /// <param name="ptInter2">交点2(若不存在返回0)</param>
    public static bool LineInterCircle(Vector2 ptStart, Vector2 ptEnd, Vector2 ptCenter, float Radius2,
       ref Vector2 ptInter1, ref Vector2 ptInter2)
    {
        float EPS = 0.00001f;
        //求线段的长度
        float fDis = Mathf.Sqrt((ptEnd.x - ptStart.x) * (ptEnd.x - ptStart.x) + (ptEnd.y - ptStart.y) * (ptEnd.y - ptStart.y));
        Vector2 d = new Vector2();
        d.x = (ptEnd.x - ptStart.x) / fDis;
        d.y = (ptEnd.y - ptStart.y) / fDis;
        Vector2 E = new Vector2();
        E.x = ptCenter.x - ptStart.x;
        E.y = ptCenter.y - ptStart.y;
        float a = E.x * d.x + E.y * d.y;
        float a2 = a * a;
        float e2 = E.x * E.x + E.y * E.y;
        if ((Radius2 - e2 + a2) < 0)
        {
            return false;
        }
        else
        {
            float f = Mathf.Sqrt(Radius2 - e2 + a2);
            float t = a - f;
            if (((t - 0.0) > -EPS) && (t - fDis) < EPS)
            {
                ptInter1.x = ptStart.x + t * d.x;
                ptInter1.y = ptStart.y + t * d.y;
            }
            t = a + f;
            if (((t - 0.0) > -EPS) && (t - fDis) < EPS)
            {
                ptInter2.x = ptStart.x + t * d.x;
                ptInter2.y = ptStart.y + t * d.y;
            }
            return true;
        }
    }


    public static class Movement
    {
        public static float AccelerateSpeed(float speed, float acceleration, float maxSpeed, bool left)
        {
            if (left && speed < -maxSpeed)
            {
                return speed;
            }
            if (!left && speed > maxSpeed)
            {
                return speed;
            }
            return (!left) ? Mathf.Min(maxSpeed, speed + acceleration * Time.deltaTime) : Mathf.Max(-maxSpeed, speed - acceleration * Time.deltaTime);
        }

        public static float DecelerateSpeed(float speed, float deceleration)
        {
            return (speed <= 0f) ? Mathf.Min(0f, speed + deceleration * Time.deltaTime) : Mathf.Max(0f, speed - deceleration * Time.deltaTime);
        }

        public static float ApplyGravity(float speed, float gravity, float maxSpeed)
        {
            return Mathf.Max(-maxSpeed, speed - gravity * Time.deltaTime);
        }
    }

    public static class Line
    {
        public static Vector3 ClosestPointOnLineSegmentToPoint(Vector3 p1, Vector3 p2, Vector3 p)
        {
            Vector3 vector = p2 - p1;
            if (vector.sqrMagnitude < Mathf.Epsilon)
            {
                return (p1 + p2) / 2f;
            }
            float num = ((p.x - p1.x) * vector.x + (p.y - p1.y) * vector.y) / vector.sqrMagnitude;
            num = Mathf.Clamp01(num);
            return Vector3.Lerp(p1, p2, num);
        }

        public static float DistancePointToLine(Vector3 p1, Vector3 p2, Vector3 p)
        {
            return Vector3.Distance(p, Util.Line.ClosestPointOnLineSegmentToPoint(p1, p2, p));
        }
    }

    public static class Normal
    {
        public static bool WithinDegrees(Vector2 normal1, Vector2 normal2, float degrees)
        {
            return Vector3.Dot(normal1, normal2) >= Mathf.Cos(Mathf.Deg2Rad * degrees);
        }
    }

    public static class Vector
    {
        public static Vector2 ApplyCircleDeadzone(Vector2 axis, float deadzoneRadius)
        {
            if (axis.magnitude < deadzoneRadius)
            {
                return Vector2.zero;
            }
            return axis.normalized * (axis.magnitude - deadzoneRadius) / (1f - deadzoneRadius);
        }

        public static Vector2 ApplyRectangleDeadzone(Vector2 axis, float deadzoneX, float deadzoneY)
        {
            axis.x = Mathf.Sign(axis.x) * Mathf.Max(Mathf.Abs(axis.x) - deadzoneX, 0f) / (1f - deadzoneX);
            axis.y = Mathf.Sign(axis.y) * Mathf.Max(Mathf.Abs(axis.y) - deadzoneY, 0f) / (1f - deadzoneY);
            return axis;
        }

        public static Vector3 Abs(Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        public static Vector3 Divide(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        public static Vector2 RotateTowards(Vector2 angleVector, Vector2 targetVector, float delta)
        {
            float num = Util.Angle.AngleFromVector(angleVector);
            float target = Util.Angle.AngleFromVector(targetVector);
            num = Mathf.MoveTowardsAngle(num, target, Time.deltaTime * delta);
            return Util.Angle.VectorFromAngle(num) * angleVector.magnitude;
        }

        public static bool PointInTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            bool flag = Util.Vector.sign(pt, v1, v2) < 0f;
            bool flag2 = Util.Vector.sign(pt, v2, v3) < 0f;
            bool flag3 = Util.Vector.sign(pt, v3, v1) < 0f;
            return flag == flag2 && flag2 == flag3;
        }

        private static float sign(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
        }

        public static float Distance(Vector3 start, Vector3 target)
        {
            float num = start.x - target.x;
            float num2 = start.y - target.y;
            float num3 = start.z - target.z;
            return Mathf.Sqrt(num * num + num2 * num2 + num3 * num3);
        }

        public static float Distance(Vector3 start, Vector2 target)
        {
            float num = start.x - target.x;
            float num2 = start.y - target.y;
            float z = start.z;
            return Mathf.Sqrt(num * num + num2 * num2 + z * z);
        }
    }

    public static class Angle
    {
        public static float Wrap(float angle)
        {
            while (angle >= 360f)
            {
                angle -= 360f;
            }
            while (angle < 0f)
            {
                angle += 360f;
            }
            return angle;
        }

        public static float Wrap180(float angle)
        {
            while (angle >= 180f)
            {
                angle -= 360f;
            }
            while (angle < -180f)
            {
                angle += 360f;
            }
            return angle;
        }

        public static float Difference(float value1, float value2)
        {
            return Mathf.Min(Mathf.Abs(value1 - value2), 360f - Mathf.Abs(value1 - value2));
        }

        public static float AngleSubtract(float start, float target)
        {
            float num = Util.Angle.Wrap(target);
            float num2 = Util.Angle.Wrap(start);
            int num3 = (num <= num2) ? -1 : 1;
            if (num3 == -1)
            {
                num = Util.Angle.Wrap(start);
                num2 = Util.Angle.Wrap(target);
            }
            if (Mathf.Abs(num - num2) < 360f - Mathf.Abs(num - num2))
            {
                return Mathf.Abs(num - num2) * (float)num3;
            }
            return (360f - Math.Abs(num - num2)) * (float)num3 * -1f;
        }

        public static float RotateTowards(float startDegrees, float targetDegrees, float degrees)
        {
            if (Mathf.Abs(degrees) > Util.Angle.Difference(startDegrees, targetDegrees))
            {
                return targetDegrees;
            }
            if (Util.Angle.Difference(Util.Angle.Wrap(startDegrees + degrees), targetDegrees) < Util.Angle.Difference(Util.Angle.Wrap(startDegrees - degrees), targetDegrees))
            {
                return Util.Angle.Wrap(startDegrees + degrees);
            }
            return Util.Angle.Wrap(startDegrees - degrees);
        }

        public static Vector2 Rotate(Vector2 v, float angle)
        {
            if (angle == 0f)
            {
                return v;
            }
            float f = angle * 0.0174532924f;
            float num = Mathf.Cos(f);
            float num2 = Mathf.Sin(f);
            return new Vector2(v.x * num - v.y * num2, v.x * num2 + v.y * num);
        }

        public static Vector2 Unrotate(Vector2 v, float angle)
        {
            if (angle == 0f)
            {
                return v;
            }
            return Util.Angle.Rotate(v, -angle);
        }

        public static float AngleFromVector(Vector2 delta)
        {
            delta.Normalize();
            return Mathf.Atan2(delta.y, delta.x) * 57.29578f;
        }

        public static float AngleFromDirection(Vector2 delta)
        {
            return Mathf.Atan2(delta.y, delta.x) * 57.29578f;
        }

        public static Vector2 VectorFromAngle(float angle)
        {
            return new Vector2(Mathf.Cos(angle * 0.0174532924f), Mathf.Sin(angle * 0.0174532924f));
        }
    }

    public static class Float
    {
        public static float Normalize(float x)
        {
            return (x != 0f) ? Mathf.Sign(x) : 0f;
        }

        public static float MoveTowards(float start, float target, float distance)
        {
            if (target > start)
            {
                return Mathf.Min(target, start + distance);
            }
            return Mathf.Max(target, start - distance);
        }

        public static float ClampedAdd(float start, float offset, float min, float max)
        {
            if (offset > 0f && start < max)
            {
                return Mathf.Min(max, start + offset);
            }
            if (offset < 0f && start > min)
            {
                return Mathf.Max(min, start + offset);
            }
            return start;
        }

        public static float ClampedSubtract(float start, float offset, float min, float max)
        {
            if (start < min)
            {
                return Mathf.Min(min, start - offset);
            }
            if (start > max)
            {
                return Mathf.Max(max, start - offset);
            }
            return start;
        }

        public static float ClampedDecrease(float start, float amount, float min, float max)
        {
            if (start < min)
            {
                return Mathf.Min(min, start + amount);
            }
            if (start > max)
            {
                return Mathf.Max(max, start - amount);
            }
            return start;
        }

        public static float Wrap(float value, float min, float max)
        {
            value -= min;
            max -= min;
            value -= Mathf.Floor(value / max) * max;
            return value + min;
        }

        public static float AbsoluteMax(float a, float b)
        {
            return (Mathf.Abs(a) <= Mathf.Abs(b)) ? b : a;
        }

        public static float AbsoluteMin(float a, float b)
        {
            return (Mathf.Abs(a) >= Mathf.Abs(b)) ? b : a;
        }

        public static float AbsoluteDifference(float a, float b)
        {
            return Mathf.Abs(a - b);
        }
    }

    public static class Physics
    {
        public static float SpeedFromHeightAndGravity(float gravity, float height)
        {
            return Mathf.Sqrt(height * 2f * gravity);
        }
    }

    public static class Rectangle
    {
        public static Rect Absolute(Rect rect)
        {
            return Rect.MinMaxRect(Mathf.Min(rect.xMin, rect.xMax), Mathf.Min(rect.yMin, rect.yMax), Mathf.Max(rect.xMin, rect.xMax), Mathf.Max(rect.yMin, rect.yMax));
        }
    }

    public static class Int
    {
        public static int GreatestCommonDenominator(int x, int y)
        {
            while (y != 0)
            {
                int num = x % y;
                x = y;
                y = num;
            }
            return x;
        }
    }
}