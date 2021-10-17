﻿using System;
using System.Runtime.InteropServices;

namespace maxstAR
{
    internal class NativeAPI
    {
#if UNITY_IOS && !UNITY_EDITOR
        const string MaxstARLibName = "__Internal";
#else
        const string MaxstARLibName = "MaxstAR";
#endif

        #region -- System setting
        [DllImport(MaxstARLibName)]
        public static extern void maxst_init(string licenseKey);
        #endregion

        #region -- MaxstAR setting
        [DllImport(MaxstARLibName)]
        public static extern void maxst_getVersion(byte[] versionBytes, int bytesLength);

        [DllImport(MaxstARLibName)]
        public static extern void maxst_onSurfaceChanged(int surfaceWidth, int surfaceHeight);

        [DllImport(MaxstARLibName)]
        public static extern void maxst_setScreenOrientation(int orientation);
        #endregion

        #region -- Camera device setting
        [DllImport(MaxstARLibName)]
        public static extern int maxst_CameraDevice_start(int cameraId, int preferredWidth, int preferredHeight);

        [DllImport(MaxstARLibName)]
        public static extern int maxst_CameraDevice_startSimulator(string foloderPath);

        [DllImport(MaxstARLibName)]
        public static extern int maxst_CameraDevice_stop();

        [DllImport(MaxstARLibName)]
        public static extern void maxst_CameraDevice_flipVideo(int direction, bool toggle);

        [DllImport(MaxstARLibName)]
        public static extern bool maxst_CameraDevice_isVideoFlipped(int direction);

        [DllImport(MaxstARLibName)]
        public static extern bool maxst_CameraDevice_setClippingPlane(float near, float far);

        [DllImport(MaxstARLibName)]
        public static extern int maxst_CameraDevice_getWidth();

        [DllImport(MaxstARLibName)]
        public static extern int maxst_CameraDevice_getHeight();

        [DllImport(MaxstARLibName)]
        public static extern void maxst_CameraDevice_getProjectionMatrix(float[] matrix);

        [DllImport(MaxstARLibName)]
        public static extern void maxst_CameraDevice_getBackgroundPlaneGeometry(float[] values);

        [DllImport(MaxstARLibName)]
        public static extern bool maxst_CameraDevice_isFusionSupported(int type);

        [DllImport(MaxstARLibName)]
        public static extern void maxst_CameraDevice_getFusionPose(int type, float[] vps_pose, float[] local_pose, float[] convertPose);

        [DllImport(MaxstARLibName)]
        public static extern bool maxst_CameraDevice_setIntrinsic(int width, int height, float fx, float fy, float px, float py);

        [DllImport(MaxstARLibName)]
        public static extern void maxst_CameraDevice_setFusionCameraIntrinsic(float fx, float fy, float px, float py);

        [DllImport(MaxstARLibName)]
        public static extern bool maxst_CameraDevice_requestAsyncImage();

        [DllImport(MaxstARLibName)]
        public static extern bool maxst_CameraDevice_setAsyncImage(bool isEnable);

        [DllImport(MaxstARLibName)]
        public static extern void maxst_CameraDevice_setNewFrameAndPoseAndTimestamp(byte[] data, int length, int width, int height, int format, float[] pose, ulong timestamp);

        [DllImport(MaxstARLibName)]
        public static extern void maxst_CameraDevice_setNewVPSCameraPoseAndTimestamp(float[] pose, ulong timestamp);

        [DllImport(MaxstARLibName)]
        public static extern void maxst_CameraDevice_setSyncCameraFrameAndPoseAndTimestamp(IntPtr[] textureIds, int textureLength, float[] pose, ulong timestamp, int trackingState, int trackingFailureReason);

        #endregion

        #region -- TrackerManager settings
        [DllImport(MaxstARLibName)]
        public static extern void maxst_TrackerManager_startTracker();

        [DllImport(MaxstARLibName)]
        public static extern void maxst_TrackerManager_stopTracker();

        [DllImport(MaxstARLibName)]
        public static extern void maxst_TrackerManager_destroyTracker();

        [DllImport(MaxstARLibName)]
        public static extern void maxst_TrackerManager_refreshTracker();

        [DllImport(MaxstARLibName)]
        public static extern void maxst_TrackerManager_addTrackerData(string trackingFileName, bool isAndroidAssetFile = false);

        [DllImport(MaxstARLibName)]
        public static extern void maxst_TrackerManager_removeTrackerData(string trackingFileName = "");

        [DllImport(MaxstARLibName)]
        public static extern void maxst_TrackerManager_replaceServerIP(string serverIP);

        [DllImport(MaxstARLibName)]
        public static extern void maxst_TrackerManager_loadTrackerData();

        [DllImport(MaxstARLibName)]
        public static extern void maxst_TrackerManager_updateFrame();
        
        [DllImport(MaxstARLibName)]
        public static extern ulong maxst_TrackerManager_getARFrame();

        [DllImport(MaxstARLibName)]
        public static extern int maxst_TrackerManager_getServerQueryTime(byte[] timeString);
        #endregion

        #region -- Image Extractor
        [DllImport(MaxstARLibName)]
        public static extern int maxst_TrackedImage_getIndex(ulong Image_cPtr);

        [DllImport(MaxstARLibName)]
        public static extern int maxst_TrackedImage_getWidth(ulong Image_cPtr);

        [DllImport(MaxstARLibName)]
        public static extern int maxst_TrackedImage_getHeight(ulong Image_cPtr);

        [DllImport(MaxstARLibName)]
        public static extern int maxst_TrackedImage_getLength(ulong Image_cPtr);

        [DllImport(MaxstARLibName)]
        public static extern int maxst_TrackedImage_getFormat(ulong Image_cPtr);

        [DllImport(MaxstARLibName)]
        public static extern bool maxst_TrackedImage_isTextureId(ulong Image_cPtr);

        [DllImport(MaxstARLibName)]
        public static extern bool maxst_TrackedImage_getTextureId(ulong Image_cPtr, IntPtr[] textureIds);

        [DllImport(MaxstARLibName)]
        public static extern void maxst_TrackedImage_getData(ulong Image_cPtr, byte[] buffer, int size);

        [DllImport(MaxstARLibName)]
        public static extern ulong maxst_TrackedImage_getDataPtr(ulong Image_cPtr);

        [DllImport(MaxstARLibName)]
        public static extern void maxst_TrackedImage_getYuv420spY_UVPtr(ulong Image_cPtr, out IntPtr y_Ptr, out IntPtr uv_Ptr);

        [DllImport(MaxstARLibName)]
        public static extern void maxst_TrackedImage_getYuv420spY_U_VPtr(ulong Image_cPtr, out IntPtr y_Ptr, out IntPtr u_Ptr, out IntPtr v_Ptr);

        [DllImport(MaxstARLibName)]
        public static extern void maxst_TrackedImage_getYuv420_888YUVPtr(ulong Image_cPtr, out IntPtr y_Ptr, out IntPtr u_Ptr, out IntPtr v_Ptr, bool supportRG16Texture);
        #endregion

        [DllImport(MaxstARLibName)]
        public static extern ulong maxst_ARFrame_getTimeStamp(ulong arFrame_cPtr);

        [DllImport(MaxstARLibName)]
        public static extern ulong maxst_ARFrame_getTrackedImage(ulong arFrame_cPtr);

        [DllImport(MaxstARLibName)]
        public static extern void maxst_ARFrame_getTransform(ulong arFrame_cPtr, float[] transform);

        [DllImport(MaxstARLibName)]
        public static extern int maxst_ARFrame_getARTrackingState(ulong arFrame_cPtr);

        [DllImport(MaxstARLibName)]
        public static extern int maxst_ARFrame_getARLocationRecognitionState(ulong arFrame_cPtr);

        [DllImport(MaxstARLibName)]
        public static extern int maxst_ARFrame_getARTrackingFailureReason(ulong arFrame_cPtr);

        [DllImport(MaxstARLibName)]
        public static extern void maxst_ARFrame_getARLocalizerLocation(ulong arFrame_cPtr, byte[] data);

    }
}
